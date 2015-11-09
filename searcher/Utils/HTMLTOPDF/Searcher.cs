using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace patterns.Utils.HTMLTOPDF
{
    public class Searcher
    {
        public byte[] find(byte[] pdf, string text)
        {
            int coincidences = 0;
            if (String.IsNullOrEmpty(text))
                return new byte[0];

            MyLocationTextExtractionStrategy txt = new MyLocationTextExtractionStrategy(text);
            Stream inputPdfStream = new MemoryStream(pdf);
            MemoryStream outPdfStream = new MemoryStream();
            PdfReader reader = new PdfReader(inputPdfStream);
            using (PdfStamper stamper = new PdfStamper(reader, outPdfStream))
            {
                for (int i = 1; i < reader.NumberOfPages + 1; i++)
                {
                    var ex = PdfTextExtractor.GetTextFromPage(reader, i, txt);

                    coincidences = txt.myPoints.Count();
                    foreach (var item in txt.myPoints)
                    {

                        //Create a rectangle for the highlight. NOTE: Technically this isn't used but it helps with the quadpoint calculation
                        iTextSharp.text.Rectangle rect = new iTextSharp.text.Rectangle(item.Rect);
                        //Create an array of quad points based on that rectangle. NOTE: The order below doesn't appear to match the actual spec but is what Acrobat produces
                        float[] quad = { rect.Left, rect.Bottom, rect.Right, rect.Bottom, rect.Left, rect.Top, rect.Right, rect.Top };

                        //Create our hightlight
                        PdfAnnotation highlight = PdfAnnotation.CreateMarkup(stamper.Writer, rect, null, PdfAnnotation.MARKUP_HIGHLIGHT, quad);

                        //Set the color
                        highlight.Color = BaseColor.YELLOW;

                        //Add the annotation
                        stamper.AddAnnotation(highlight, 1);
                    }
                }
            }

            return outPdfStream.GetBuffer();
        }

        public void high()
        {
            ////Create a new file from our test file with highlighting
            //string highLightFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Highlighted.pdf");

            ////Bind a reader and stamper to our test PDF
            //PdfReader reader = new PdfReader(outputFile);

            //using (FileStream fs = new FileStream(highLightFile, FileMode.Create, FileAccess.Write, FileShare.None))
            //{
                
        }
    }

    public class MyLocationTextExtractionStrategy : LocationTextExtractionStrategy
    {
        public String TextToSearchFor { get; set; }
        bool find = false;

        //How to compare strings
        public System.Globalization.CompareOptions CompareOptions { get; set; }
        //Hold each coordinate
        public List<RectAndText> myPoints = new List<RectAndText>();
        public MyLocationTextExtractionStrategy(String textToSearchFor)
        {
            this.TextToSearchFor = textToSearchFor;
            this.CompareOptions = System.Globalization.CompareOptions.IgnoreCase;
        }
        //Automatically called for each chunk of text in the PDF
        public override void RenderText(TextRenderInfo renderInfo)
        {
            base.RenderText(renderInfo);

            var startPosition = System.Globalization.CultureInfo.CurrentCulture.CompareInfo.IndexOf(renderInfo.GetText(), this.TextToSearchFor, this.CompareOptions);

            var x = renderInfo.GetText().Contains(TextToSearchFor);
            //Get the bounding box for the chunk of text
            if (x)
            {
                var bottomLeft = renderInfo.GetDescentLine().GetStartPoint();
                var topRight = renderInfo.GetAscentLine().GetEndPoint();

                //Create a rectangle from it
                var rect = new iTextSharp.text.Rectangle(
                                                        bottomLeft[Vector.I1],
                                                        bottomLeft[Vector.I2],
                                                        topRight[Vector.I1],
                                                        topRight[Vector.I2]
                                                        );



                this.myPoints.Add(new RectAndText(rect, renderInfo.GetText()));
            }
        }

        public class RectAndText
        {
            public iTextSharp.text.Rectangle Rect;
            public String Text;
            public RectAndText(iTextSharp.text.Rectangle rect, String text)
            {
                this.Rect = rect;
                this.Text = text;
            }
        }
    }
}