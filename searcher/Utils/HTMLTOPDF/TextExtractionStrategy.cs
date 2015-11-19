using iTextSharp.text.pdf.parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace searcher.Utils.HTMLTOPDF
{
    public class TextExtractionStrategy : LocationTextExtractionStrategy
    {
        //TextToSearch
        public String TextToSearchFor { get; set; }
        //How to compare strings
        public System.Globalization.CompareOptions CompareOptions { get; set; }
        //Hold each coordinate
        public List<RectAndText> myPoints = new List<RectAndText>();
        //Text buffer
        private StringBuilder result = new StringBuilder();
        //Store last used properties
        private Vector lastBaseLine;
        private Vector lastAscentLine;
        //Buffer of lines of text and their Y coordinates. NOTE, these should be exposed as properties instead of fields but are left as is for simplicity's sake
        public List<string> strings = new List<String>();
        public List<float> baselines = new List<float>();
        Vector startPositionWord, endPositionWord;
        bool wordended = false;

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


        //Automatically called for each chunk of text in the PDF
        public override void RenderText(TextRenderInfo renderInfo)
        {
            base.RenderText(renderInfo);

            var startPosition = System.Globalization.CultureInfo.CurrentCulture.CompareInfo.IndexOf(renderInfo.GetText(), this.TextToSearchFor, this.CompareOptions);
            var texto = renderInfo.GetText();
            var x = renderInfo.GetText().Contains(TextToSearchFor);
            //Get the bounding box for the chunk of text
            /*if (x)
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
            }*/

            //This code assumes that if the baseline changes then we're on a newline
            Vector curBaseline = renderInfo.GetBaseline().GetStartPoint();
            Vector curBaseline2 = renderInfo.GetAscentLine().GetEndPoint();

            //See if the baseline has changed
            if ((this.lastBaseLine != null) && (curBaseline[Vector.I2] != lastBaseLine[Vector.I2]))
            {
                //See if we have text and not just whitespace
                if ((!String.IsNullOrWhiteSpace(this.result.ToString())))
                {
                    //Mark the previous line as done by adding it to our buffers
                    this.baselines.Add(this.lastBaseLine[Vector.I2]);
                    this.strings.Add(this.result.ToString());

                    if (this.result.ToString().Contains(TextToSearchFor))
                    {
                        //Create a rectangle from it
                        var rect = new iTextSharp.text.Rectangle(startPositionWord[Vector.I1],
                                                            startPositionWord[Vector.I2],
                                                            lastAscentLine[Vector.I1],
                                                            lastAscentLine[Vector.I2]);

                        this.myPoints.Add(new RectAndText(rect, this.result.ToString()));
                        startPositionWord = null;
                        endPositionWord = null;
                    }
                }
                //Reset our "line" buffer
                this.result.Clear();
                wordended = true;
            }

            //Append the current text to our line buffer
            if (!string.IsNullOrWhiteSpace(renderInfo.GetText()))
                this.result.Append(renderInfo.GetText());

            if (!string.IsNullOrWhiteSpace(result.ToString()) && wordended)
            {
                wordended = false;
                startPositionWord = renderInfo.GetDescentLine().GetStartPoint();
            }
            //Reset the last used line
            this.lastBaseLine = curBaseline;
            this.lastAscentLine = curBaseline2;
        }
    }
}