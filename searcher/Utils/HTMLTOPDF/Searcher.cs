using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace searcher.Utils.HTMLTOPDF
{
    public class Searcher
    {
        public class Respuesta
        {
            public byte[] file { get; set; }
            public int coincidences { get; set; }
            public List<String> text { get; set; }

            public Respuesta()
            {
                this.coincidences = 0;
                this.text = new List<string>();
                this.file = new byte[0];
            }
        }
        public Respuesta find(byte[] pdf, string text)
        {
            int coincidences = 0;
            List<string> cadenas = new List<string>();
            if (String.IsNullOrEmpty(text))
                return new Respuesta();

            MyLocationTextExtractionStrategy txt = new MyLocationTextExtractionStrategy(text);
            LocationTextExtractionStrategyEx st = new LocationTextExtractionStrategyEx();
            Stream inputPdfStream = new MemoryStream(pdf);
            MemoryStream outPdfStream = new MemoryStream();

            PdfReader reader = new PdfReader(inputPdfStream);
            using (PdfStamper stamper = new PdfStamper(reader, outPdfStream))
            {
                for (int i = 1; i < reader.NumberOfPages + 1; i++)
                {
                    txt = new MyLocationTextExtractionStrategy(text);
                    var ex = PdfTextExtractor.GetTextFromPage(reader, i, txt);
                    txt.strings.ForEach(item => { if (item.Contains(text)) coincidences++; });                    
                    //coincidences = txt.myPoints.Count();
                    foreach (var item in txt.myPoints)
                    {

                        //Create a rectangle for the highlight. NOTE: Technically this isn't used but it helps with the quadpoint calculation
                        iTextSharp.text.Rectangle rect = new iTextSharp.text.Rectangle(item.Rect);
                        //Create an array of quad points based on that rectangle. NOTE: The order below doesn't appear to match the actual spec but is what Acrobat produces
                        //float[] quad = { rect.Left, rect.Bottom, rect.Right, rect.Bottom, rect.Left, rect.Top, rect.Right, rect.Top };

                        //Create our hightlight
                        //PdfAnnotation highlight = PdfAnnotation.CreateMarkup(stamper.Writer, rect, null, PdfAnnotation.MARKUP_HIGHLIGHT, quad);

                        //Set the color
                        //highlight.Color = BaseColor.YELLOW;

                        //Add the annotation
                        //stamper.AddAnnotation(highlight, 1);                        

                        using (Document doc = new Document())
                        {
                            PdfContentByte over = stamper.GetOverContent(i);

                            over.SaveState();

                            PdfGState gs1 = new PdfGState();
                            gs1.FillOpacity = 0.3f;
                            over.SetGState(gs1);

                            over.Rectangle(rect.Left, rect.Bottom, rect.Width, rect.Height);
                            over.SetColorFill(BaseColor.YELLOW);
                            over.Fill();
                            over.SetColorStroke(BaseColor.BLUE);
                            over.Stroke();                                                         

                            over.RestoreState();                            
                            doc.Close();
                        }
                    }

                    string ex2 = PdfTextExtractor.GetTextFromPage(reader, i, st);
                    //int coinci = ex2.

                    //foreach (var item in st.TextLocationInfo)
                    //{
                    //    if (string.IsNullOrWhiteSpace(item.Text))
                    //        continue;

                    //    if (!item.Text.Contains(text))
                    //        continue;

                    //    coincidences++;
                    //    cadenas.Add(item.Text);
                    //    item.rectangle.Height = 10f;
                    //    //item.rectangle.Width = 200f;
                    //    iTextSharp.text.Rectangle rect = new iTextSharp.text.Rectangle(item.rectangle);

                    //    using (Document doc = new Document())
                    //    {
                    //        PdfContentByte over = stamper.GetOverContent(i);

                    //        over.SaveState();

                    //        PdfGState gs1 = new PdfGState();
                    //        gs1.FillOpacity = 0.3f;
                    //        over.SetGState(gs1);

                    //        over.Rectangle(rect.Left, rect.Bottom, rect.Width, rect.Height);
                    //        over.SetColorFill(BaseColor.YELLOW);
                    //        over.Fill();
                    //        over.SetColorStroke(BaseColor.BLUE);
                    //        over.Stroke();

                    //        over.RestoreState();
                    //        doc.Close();
                    //    }
                    //}
                }
            }

            //MemoryStream outPdfStream2 = new MemoryStream(outPdfStream.GetBuffer(), true);

            //using (Document doc = new Document())
            //{
            //    PdfWriter writer = PdfWriter.GetInstance(doc, outPdfStream2);
            //    doc.Open();
            //    PdfContentByte over = writer.DirectContent;

            //    over.SaveState();

            //    over.Rectangle(10, 10, 50, 50);
            //    over.SetColorFill(BaseColor.BLUE);
            //    over.Fill();


            //    PdfGState gs1 = new PdfGState();
            //    gs1.FillOpacity = 0.5f;
            //    over.SetGState(gs1);

            //    over.Rectangle(0, 0, 60, 60);
            //    over.SetColorFill(new BaseColor(255, 0, 0, 150));
            //    over.Fill();

            //    over.RestoreState();

            //    doc.Close();
            //}

            return new Respuesta() { coincidences = coincidences, file = outPdfStream.GetBuffer(), text = cadenas }; 
        }
    }

    public class MyLocationTextExtractionStrategy : LocationTextExtractionStrategy
    {
        public String TextToSearchFor { get; set; }
        bool find = false;
        //Text buffer
        private StringBuilder result = new StringBuilder();

        //Store last used properties
        private Vector lastBaseLine;

        //Buffer of lines of text and their Y coordinates. NOTE, these should be exposed as properties instead of fields but are left as is for simplicity's sake
        public List<string> strings = new List<String>();
        public List<float> baselines = new List<float>();

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

            //See if the baseline has changed
            if ((this.lastBaseLine != null) && (curBaseline[Vector.I2] != lastBaseLine[Vector.I2]))
            {
                //See if we have text and not just whitespace
                if ((!String.IsNullOrWhiteSpace(this.result.ToString())))
                {
                    //Mark the previous line as done by adding it to our buffers
                    this.baselines.Add(this.lastBaseLine[Vector.I2]);
                    this.strings.Add(this.result.ToString());
                }
                //Reset our "line" buffer
                this.result.Clear();
            }

            //Append the current text to our line buffer
            this.result.Append(renderInfo.GetText());

            //Reset the last used line
            this.lastBaseLine = curBaseline;
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

    class LocationTextExtractionStrategyEx : LocationTextExtractionStrategy
    {
        private List<TextChunk> m_locationResult = new List<TextChunk>();
        private List<TextInfo> m_TextLocationInfo = new List<TextInfo>();
        public List<TextChunk> LocationResult
        {
            get { return m_locationResult; }
        }
        public List<TextInfo> TextLocationInfo
        {
            get { return m_TextLocationInfo; }
        }

        /// <summary>
        /// Creates a new LocationTextExtracationStrategyEx
        /// </summary>
        public LocationTextExtractionStrategyEx()
        {
        }

        /// <summary>
        /// Returns the result so far
        /// </summary>
        /// <returns>a String with the resulting text</returns>
        public override String GetResultantText()
        {
            m_locationResult.Sort();

            StringBuilder sb = new StringBuilder();
            TextChunk lastChunk = null;
            TextInfo lastTextInfo = null;
            foreach (TextChunk chunk in m_locationResult)
            {
                if (lastChunk == null)
                {
                    sb.Append(chunk.Text);
                    lastTextInfo = new TextInfo(chunk);
                    m_TextLocationInfo.Add(lastTextInfo);
                }
                else
                {
                    if (chunk.sameLine(lastChunk))
                    {
                        float dist = chunk.distanceFromEndOf(lastChunk);

                        if (dist < -chunk.CharSpaceWidth)
                        {
                            sb.Append(' ');
                            lastTextInfo.addSpace();
                        }
                        //append a space if the trailing char of the prev string wasn't a space && the 1st char of the current string isn't a space
                        else if (dist > chunk.CharSpaceWidth / 2.0f && chunk.Text[0] != ' ' && lastChunk.Text[lastChunk.Text.Length - 1] != ' ')
                        {
                            sb.Append(' ');
                            lastTextInfo.addSpace();
                        }
                        sb.Append(chunk.Text);
                        lastTextInfo.appendText(chunk);
                    }
                    else
                    {
                        sb.Append('\n');
                        sb.Append(chunk.Text);
                        lastTextInfo = new TextInfo(chunk);
                        m_TextLocationInfo.Add(lastTextInfo);
                    }
                }
                lastChunk = chunk;
            }
            return sb.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="renderInfo"></param>
        public override void RenderText(TextRenderInfo renderInfo)
        {
            LineSegment segment = renderInfo.GetBaseline();
            string x = renderInfo.GetText();
            TextChunk location = new TextChunk(renderInfo.GetText(), segment.GetStartPoint(), segment.GetEndPoint(), renderInfo.GetSingleSpaceWidth(), renderInfo.GetAscentLine(), renderInfo.GetDescentLine());
            m_locationResult.Add(location);
        }

        public class TextChunk : IComparable, ICloneable
        {
            string m_text;
            Vector m_startLocation;
            Vector m_endLocation;
            Vector m_orientationVector;
            int m_orientationMagnitude;
            int m_distPerpendicular;
            float m_distParallelStart;
            float m_distParallelEnd;
            float m_charSpaceWidth;

            public LineSegment AscentLine;
            public LineSegment DecentLine;

            public object Clone()
            {
                TextChunk copy = new TextChunk(m_text, m_startLocation, m_endLocation, m_charSpaceWidth, AscentLine, DecentLine);
                return copy;
            }

            public string Text
            {
                get { return m_text; }
                set { m_text = value; }
            }
            public float CharSpaceWidth
            {
                get { return m_charSpaceWidth; }
                set { m_charSpaceWidth = value; }
            }
            public Vector StartLocation
            {
                get { return m_startLocation; }
                set { m_startLocation = value; }
            }
            public Vector EndLocation
            {
                get { return m_endLocation; }
                set { m_endLocation = value; }
            }

            /// <summary>
            /// Represents a chunk of text, it's orientation, and location relative to the orientation vector
            /// </summary>
            /// <param name="txt"></param>
            /// <param name="startLoc"></param>
            /// <param name="endLoc"></param>
            /// <param name="charSpaceWidth"></param>
            public TextChunk(string txt, Vector startLoc, Vector endLoc, float charSpaceWidth, LineSegment ascentLine, LineSegment decentLine)
            {
                m_text = txt;
                m_startLocation = startLoc;
                m_endLocation = endLoc;
                m_charSpaceWidth = charSpaceWidth;
                AscentLine = ascentLine;
                DecentLine = decentLine;

                m_orientationVector = m_endLocation.Subtract(m_startLocation).Normalize();
                m_orientationMagnitude = (int)(Math.Atan2(m_orientationVector[Vector.I2], m_orientationVector[Vector.I1]) * 1000);

                // see http://mathworld.wolfram.com/Point-LineDistance2-Dimensional.html
                // the two vectors we are crossing are in the same plane, so the result will be purely
                // in the z-axis (out of plane) direction, so we just take the I3 component of the result
                Vector origin = new Vector(0, 0, 1);
                m_distPerpendicular = (int)(m_startLocation.Subtract(origin)).Cross(m_orientationVector)[Vector.I3];

                m_distParallelStart = m_orientationVector.Dot(m_startLocation);
                m_distParallelEnd = m_orientationVector.Dot(m_endLocation);
            }

            /// <summary>
            /// true if this location is on the the same line as the other text chunk
            /// </summary>
            /// <param name="textChunkToCompare">the location to compare to</param>
            /// <returns>true if this location is on the the same line as the other</returns>
            public bool sameLine(TextChunk textChunkToCompare)
            {
                if (m_orientationMagnitude != textChunkToCompare.m_orientationMagnitude) return false;
                if (m_distPerpendicular != textChunkToCompare.m_distPerpendicular) return false;
                return true;
            }

            /// <summary>
            /// Computes the distance between the end of 'other' and the beginning of this chunk
            /// in the direction of this chunk's orientation vector.  Note that it's a bad idea
            /// to call this for chunks that aren't on the same line and orientation, but we don't
            /// explicitly check for that condition for performance reasons.
            /// </summary>
            /// <param name="other"></param>
            /// <returns>the number of spaces between the end of 'other' and the beginning of this chunk</returns>
            public float distanceFromEndOf(TextChunk other)
            {
                float distance = m_distParallelStart - other.m_distParallelEnd;
                return distance;
            }

            /// <summary>
            /// Compares based on orientation, perpendicular distance, then parallel distance
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public int CompareTo(object obj)
            {
                if (obj == null) throw new ArgumentException("Object is now a TextChunk");

                TextChunk rhs = obj as TextChunk;
                if (rhs != null)
                {
                    if (this == rhs) return 0;

                    int rslt;
                    rslt = m_orientationMagnitude - rhs.m_orientationMagnitude;
                    if (rslt != 0) return rslt;

                    rslt = m_distPerpendicular - rhs.m_distPerpendicular;
                    if (rslt != 0) return rslt;

                    // note: it's never safe to check floating point numbers for equality, and if two chunks
                    // are truly right on top of each other, which one comes first or second just doesn't matter
                    // so we arbitrarily choose this way.
                    rslt = m_distParallelStart < rhs.m_distParallelStart ? -1 : 1;

                    return rslt;
                }
                else
                {
                    throw new ArgumentException("Object is now a TextChunk");
                }
            }
        }

        public class TextInfo
        {
            public Vector TopLeft;
            public Vector BottomRight;
            private string m_Text;
            public System.util.RectangleJ rectangle;

            public string Text
            {
                get { return m_Text; }
            }

            /// <summary>
            /// Create a TextInfo.
            /// </summary>
            /// <param name="initialTextChunk"></param>
            public TextInfo(TextChunk initialTextChunk)
            {
                TopLeft = initialTextChunk.AscentLine.GetStartPoint();
                BottomRight = initialTextChunk.DecentLine.GetEndPoint();
                rectangle = initialTextChunk.AscentLine.GetBoundingRectange();
                m_Text = initialTextChunk.Text;
            }

            /// <summary>
            /// Add more text to this TextInfo.
            /// </summary>
            /// <param name="additionalTextChunk"></param>
            public void appendText(TextChunk additionalTextChunk)
            {
                BottomRight = additionalTextChunk.DecentLine.GetEndPoint();
                m_Text += additionalTextChunk.Text;
            }

            /// <summary>
            /// Add a space to the TextInfo.  This will leave the endpoint out of sync with the text.
            /// The assumtion is that you will add more text after the space which will correct the endpoint.
            /// </summary>
            public void addSpace()
            {
                m_Text += ' ';
            }


        }
    }
}