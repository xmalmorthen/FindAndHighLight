using NLog;
using System;
using System.Collections.Generic;
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

        public int data()
        {
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

        public List<FileInfo> Search(string pathToFile, string stringToSearch, string pathToSave)
        {
            _Search(new List<string>(new string[] { pathToFile }), stringToSearch, pathToSave);
            return FileInfo;
        }

        public List<FileInfo> Search(List<string> pathToFile, string stringToSearch, string pathToSave)
        {
            _Search(pathToFile, stringToSearch, pathToSave);
            return FileInfo;
        }

        private void _Search(List<string> pathToFile, string stringToSearch, string pathToSave = null)
        {
            logger.Info("Clean the return List of coincidences");
            FileInfo.Clear();
            logger.Info("initializing paramenter objects");

            object oMissing = System.Reflection.Missing.Value;
            object oTrue = true;
            object oFalse = false;

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
                    doc = word.Documents.Open(ref fileName, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oFalse, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
                    doc.Activate();
                    logger.Info("file opened and activates");

                    FileInfo fiIn = new FileInfo();
                    fiIn.FileName = item;
                    fiIn.FileNameOut = string.Format(@"{0}\{1}.pdf", pathToSave, Guid.NewGuid().ToString());
                    
                    logger.Info("searching the text on each paragraphs");
                    foreach (Word.Paragraph Paragraph in doc.Paragraphs)
                    {
                        Word.Range rng = Paragraph.Range;

                        rng.Find.Text = stringToSearch.Trim();
                        rng.Find.ClearFormatting();
                        rng.Find.Forward = true;
                        rng.Find.Replacement.ClearFormatting();
                        rng.Find.Wrap = Word.WdFindWrap.wdFindStop;

                        rng.Find.Execute(
                                        ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                                        ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                                        ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);

                        while (rng.Find.Found)
                        {
                            rng.HighlightColorIndex = Microsoft.Office.Interop.Word.WdColorIndex.wdDarkYellow;
                            rng.Font.ColorIndex = Microsoft.Office.Interop.Word.WdColorIndex.wdWhite;

                            MatchesInfo maIn = new MatchesInfo();
                            maIn.Page = (int)rng.get_Information(Microsoft.Office.Interop.Word.WdInformation.wdActiveEndAdjustedPageNumber);
                            maIn.Line = (int)rng.get_Information(Microsoft.Office.Interop.Word.WdInformation.wdFirstCharacterLineNumber);

                            fiIn.MatchesList.Add(maIn);
                            logger.Info("text matching and highlight");

                            rng.Find.Execute(
                                        ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                                        ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                                        ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
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

                    object saveOption = Word.WdSaveOptions.wdDoNotSaveChanges;
                    object originalFormat = Word.WdOriginalFormat.wdOriginalDocumentFormat;
                    object routeDocument = false;
                    ((Word._Document)doc).Close(ref saveOption, ref originalFormat, ref routeDocument);
                }
            }
            catch (Exception ex)
            {
                //TODO: Manipular errores;
                logger.Info("An exception has detected");
                logger.Error(ex);
                throw new Exception("");
                throw;
            }
            finally
            {
                ((Word._Application)word).Quit(ref oFalse, ref oMissing, ref oMissing);
            }
        }
    }
}
