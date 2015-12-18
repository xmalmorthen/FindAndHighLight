using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Word = Microsoft.Office.Interop.Word;

namespace bossTools
{
    public class Documment
    {
        Logger logger = LogManager.GetCurrentClassLogger();
        protected Word.Application word;
        protected Word.Document doc;

        private List<FileInfo> fileInfo = new List<FileInfo>();
        public List<FileInfo> FileInfo
        {
            get { return fileInfo; }
            set { fileInfo = value; }
        }

        public int data() {
            return 1;
        }

        public List<FileInfo> Search(string pathToFile, string stringToSearch)
        {            
            _Search(new List<string>(new string[] { pathToFile }), stringToSearch);
            return FileInfo;
        }

        public List<FileInfo> Search(List<string> pathToFile, string stringToSearch)
        {
            _Search(pathToFile, stringToSearch);
            return FileInfo;
        }

        public void Search(string pathToFile, string stringToSearch, string pathToSave)
        {
            _Search(new List<string>(new string[] { pathToFile }), stringToSearch, pathToSave);
        }

        public List<FileInfo> Search(List<string> pathToFile, string stringToSearch, string pathToSave)
        {
            _Search(pathToFile, stringToSearch, pathToSave);
            return FileInfo;
        }
        string exc = "";
        private void _Search(List<string> pathToFile, string stringToSearch, string pathToSave = null)
        {
            logger.Info("Clean the return List of coincidences");
            FileInfo.Clear();
            logger.Info("initializing paramenter objects");
            object missing = System.Type.Missing;
            object saveOption = Word.WdSaveOptions.wdDoNotSaveChanges;
            object originalFormat = Word.WdOriginalFormat.wdOriginalDocumentFormat;
            object routeDocument = false;            
            
            try
            {
                logger.Info("instantiating word app");
                word = new Word.Application();
                logger.Info("instantiating document of word app");
                doc = new Word.Document();

                logger.Info("reading each files on path ={0}", pathToFile);
                foreach (string item in pathToFile)
                {                    
                    object fileName = item;

                    logger.Info("opening the file");
                    doc = word.Documents.Open(ref fileName, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing,ref routeDocument, ref missing, ref missing, ref missing, ref missing);
                    doc.Activate();
                    logger.Info("file opened and activates");

                    object findText = stringToSearch;
                    
                    FileInfo fiIn = new FileInfo();
                    fiIn.FileName = item;
                    fiIn.FileNameOut = string.Format(@"{0}\{1}.pdf", pathToSave, Guid.NewGuid().ToString());

                    logger.Info("searching the text on each paragraphs");
                    foreach (Word.Paragraph Paragraph in doc.Paragraphs)
                    {                        
                        Word.Range rng = Paragraph.Range;
                        
                        if (rng.Find.Execute(ref findText, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing))
                        {

                            rng.HighlightColorIndex = Microsoft.Office.Interop.Word.WdColorIndex.wdDarkYellow;
                            rng.Font.ColorIndex = Microsoft.Office.Interop.Word.WdColorIndex.wdWhite;

                            MatchesInfo maIn = new MatchesInfo();
                            maIn.Page = (int)rng.get_Information(Microsoft.Office.Interop.Word.WdInformation.wdActiveEndAdjustedPageNumber);
                            maIn.Line = (int)rng.get_Information(Microsoft.Office.Interop.Word.WdInformation.wdFirstCharacterLineNumber);

                            fiIn.MatchesList.Add(maIn);
                            logger.Info("text matching and highlight");
                        }
                    }
                    
                    FileInfo.Add(fiIn);
                    
                    if (!string.IsNullOrEmpty(pathToSave))
                    {
                        logger.Info("exporting the file");
                        doc.ExportAsFixedFormat(
                                fiIn.FileNameOut,
                                Word.WdExportFormat.wdExportFormatPDF,
                                OptimizeFor: Word.WdExportOptimizeFor.wdExportOptimizeForOnScreen,
                                BitmapMissingFonts: true, DocStructureTags: false);
                        logger.Info("file exported");
                    }
                }                
            }
            catch (Exception ex)
            {
                //TODO: Manipular errores;
                logger.Info("An exception has detected");
                logger.Error(ex);
                throw new Exception(exc);
            }
            finally {
                ((Word._Document)doc).Close(ref saveOption, ref originalFormat, ref routeDocument);
            }
        }
    }
}
