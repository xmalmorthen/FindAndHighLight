using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using HiQPdf;
using System.Web.Mvc;
using System.Text;

namespace searcher.Utils
{
    public class ConvertHtmlToString
    {

        public String TituloSistema { get; set; }
        public String PiePagina { get; set; }
        public String NombreCompleto { get; set; }
        private const String HIQPDF_SERIAL = "s/va4uPX-1f/a0cHS-wcqFnYOT-gpOHk4qL-h5OAgp2C-gZ2KioqK";
        //private void SetHeader(PdfDocumentControl htmlToPdfDocument, header header)
        //{
        //    // enable header display
        //    htmlToPdfDocument.Header.Enabled = true;
        //    // set header height
        //    htmlToPdfDocument.Header.Height = header.height;
        //    float pdfPageWidth =
        //        htmlToPdfDocument.PageOrientation == PdfPageOrientation.Portrait ?
        //                htmlToPdfDocument.PageSize.Width : htmlToPdfDocument.PageSize.Height;
        //    float headerWidth = pdfPageWidth - htmlToPdfDocument.Margins.Left
        //                - htmlToPdfDocument.Margins.Right;
        //    float headerHeight = htmlToPdfDocument.Header.Height;
        //    PdfHtml headerHtml = new PdfHtml(0, 0, header.url);
        //    headerHtml.FitDestHeight = true;
        //    htmlToPdfDocument.Header.Layout(headerHtml);
        //    //PdfRectangle borderRectangle = new PdfRectangle(1, 1, headerWidth - 2, headerHeight - 2);
        //    //borderRectangle.LineStyle.LineWidth = 0.5f;
        //    ////borderRectangle.ForeColor = System.Drawing.Color.Navy;
        //    //htmlToPdfDocument.Header.Layout(borderRectangle);
        //}
        //private void SetHeader(PdfDocumentControl htmlToPdfDocument, headerHtml header)
        //{
        //    // enable header display
        //    htmlToPdfDocument.Header.Enabled = true;
        //    // set header height
        //    htmlToPdfDocument.Header.Height = header.height;
        //    float pdfPageWidth =
        //        htmlToPdfDocument.PageOrientation == PdfPageOrientation.Portrait ?
        //                htmlToPdfDocument.PageSize.Width : htmlToPdfDocument.PageSize.Height;
        //    float headerWidth = pdfPageWidth - htmlToPdfDocument.Margins.Left
        //                - htmlToPdfDocument.Margins.Right;
        //    float headerHeight = htmlToPdfDocument.Header.Height;
        //    PdfHtml headerHtml = new PdfHtml(0, 0, header.html, "");
        //    headerHtml.FitDestHeight = true;
        //    htmlToPdfDocument.Header.Layout(headerHtml);
        //    //PdfRectangle borderRectangle = new PdfRectangle(1, 1, headerWidth - 2, headerHeight - 2);
        //    //borderRectangle.LineStyle.LineWidth = 0.5f;
        //    ////borderRectangle.ForeColor = System.Drawing.Color.Navy;
        //    //htmlToPdfDocument.Header.Layout(borderRectangle);
        //}
        private void SetHeader(PdfDocumentControl htmlToPdfDocument)
        {
            // enable header display
            htmlToPdfDocument.Header.Enabled = true;
            // set header height
            htmlToPdfDocument.Header.Height = 50;
            float pdfPageWidth =
                htmlToPdfDocument.PageOrientation == PdfPageOrientation.Portrait ?
                        htmlToPdfDocument.PageSize.Width : htmlToPdfDocument.PageSize.Height;
            float headerWidth = pdfPageWidth - htmlToPdfDocument.Margins.Left
                        - htmlToPdfDocument.Margins.Right;
            float headerHeight = htmlToPdfDocument.Header.Height;
            PdfHtml headerHtml = new PdfHtml(0, 0, "http://localhost:1579/Html/Header");
            headerHtml.FitDestHeight = true;
            htmlToPdfDocument.Header.Layout(headerHtml);
            //PdfRectangle borderRectangle = new PdfRectangle(1, 1, headerWidth - 2, headerHeight - 2);
            //borderRectangle.LineStyle.LineWidth = 0.5f;
            ////borderRectangle.ForeColor = System.Drawing.Color.Navy;
            //htmlToPdfDocument.Header.Layout(borderRectangle);
        }
        public string RenderRazorViewToString(string viewName, object model, ControllerContext ctx)
        {
            ctx.Controller.ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ctx, viewName);
                var viewContext = new ViewContext(ctx, viewResult.View, ctx.Controller.ViewData, ctx.Controller.TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ctx, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }

        private HtmlToPdf GetHtmlToPdf(float Bottom, float Top, float Left, float Right)
        {
            HiQPdf.HtmlToPdf docpdf = new HtmlToPdf();
            docpdf.Document.Margins.Bottom = Bottom;
            docpdf.Document.Margins.Top = Top;
            docpdf.Document.Margins.Left = Left;
            docpdf.Document.Margins.Right = Right;
            docpdf.SerialNumber = HIQPDF_SERIAL;
            return docpdf;
        }
        //public byte[] GenerarPDF(string viewName, object model, ControllerContext ctx, PdfPageSize pageSize, PdfPageOrientation orientation, float bottom, float top, float left, float right, footer footer = null, header header = null)
        //{
        //    if (footer == null) footer = new footer();
        //    if (header == null) header = new header();
        //    String View = RenderRazorViewToString(viewName, model, ctx);
        //    HtmlToPdf docpdf = GetHtmlToPdf(bottom, top, left, right);
        //    docpdf.Document.PageSize = pageSize == null ? PdfPageSize.A4 : pageSize;
        //    docpdf.Document.PageOrientation = orientation == null ? PdfPageOrientation.Portrait : orientation;
        //    if (footer.hasFooter)
        //        SetFooterFirmantes(docpdf.Document,footer);
        //    if (header.hasHeader)
        //        SetHeader(docpdf.Document, header); 
        //    byte[] archivo = docpdf.ConvertHtmlToPdfDocument(View, "").WriteToMemory();
        //    return archivo;
        //}
        //public byte[] GenerarPDFwithHeaderHTML(string viewName, object model, ControllerContext ctx, PdfPageSize pageSize, PdfPageOrientation orientation, float bottom, float top, float left, float right, footer footer = null, headerHtml header = null)
        //{
        //    if (footer == null) footer = new footer();
        //    if (header == null) header = new headerHtml();
        //    String View = RenderRazorViewToString(viewName, model, ctx);
        //    HtmlToPdf docpdf = GetHtmlToPdf(bottom, top, left, right);
        //    docpdf.Document.PageSize = pageSize == null ? PdfPageSize.A4 : pageSize;
        //    docpdf.Document.PageOrientation = orientation == null ? PdfPageOrientation.Portrait : orientation;
        //    if (footer.hasFooter)
        //        SetFooterFirmantes(docpdf.Document, footer);
        //    if (header.hasHeader)
        //        SetHeader(docpdf.Document, header);
        //    byte[] archivo = docpdf.ConvertHtmlToPdfDocument(View, "").WriteToMemory();
        //    return archivo;
        //}
        //public byte[] GenerarPDFFormHTML(string html, PdfPageSize pageSize, PdfPageOrientation orientation, float bottom, float top, float left, float right, footer footer = null, bool hasHeader = false)
        //{
        //    if (footer == null) footer = new footer();
        //    String View = html;
        //    HtmlToPdf docpdf = GetHtmlToPdf(bottom, top, left, right);
        //    docpdf.Document.PageSize = pageSize == null ? PdfPageSize.A4 : pageSize;
        //    docpdf.Document.PageOrientation = orientation == null ? PdfPageOrientation.Portrait : orientation;
        //    if (footer.hasFooter)
        //        SetFooterFirmantes(docpdf.Document, footer);
        //    if (hasHeader)
        //        SetHeader(docpdf.Document);
        //    byte[] archivo = docpdf.ConvertHtmlToPdfDocument(View, "").WriteToMemory();
        //    return archivo;
        //}

        public byte[] GenerarPDF_Blanco(string viewName, object model, ControllerContext ctx)
        {
            String View = RenderRazorViewToString(viewName, model, ctx);
            HtmlToPdf docpdf = GetHtmlToPdf(0, 0, 0, 0);
            byte[] archivo = docpdf.ConvertHtmlToPdfDocument(View, "").WriteToMemory();
            return archivo;
        }


        public byte[] GenerarPDF_Horizontal(string viewName, object model, ControllerContext ctx, PdfPageSize pageSize, PdfPageOrientation orientation, float bottom, float top, float left, float right, bool hasFooter = true, bool hasHeader = false)
        {
            String View = RenderRazorViewToString(viewName, model, ctx);
            HiQPdf.HtmlToPdf docpdf = GetHtmlToPdf(bottom, top, left, right);
            docpdf.Document.PageOrientation = PdfPageOrientation.Landscape;
            SetFooter(docpdf.Document);
            if (hasFooter)
                SetFooter(docpdf.Document);
            if (hasHeader)
                SetHeader(docpdf.Document);
            byte[] archivo = docpdf.ConvertHtmlToPdfDocument(View, "").WriteToMemory();
            return archivo;
        }

        private void SetFooter(PdfDocumentControl htmlToPdfDocument)
        {
            // enable footer display
            htmlToPdfDocument.Footer.Enabled = true;

            // set footer height
            htmlToPdfDocument.Footer.Height = 15;
            // set footer background color
            htmlToPdfDocument.Footer.BackgroundColor = System.Drawing.Color.WhiteSmoke;

            float pdfPageWidth = htmlToPdfDocument.PageOrientation == PdfPageOrientation.Portrait ?
                    htmlToPdfDocument.PageSize.Width : htmlToPdfDocument.PageSize.Height;

            float footerWidth = pdfPageWidth - htmlToPdfDocument.Margins.Left -
                        htmlToPdfDocument.Margins.Right;
            float footerHeight = htmlToPdfDocument.Footer.Height;

            // layout HTML in footer
            if (String.IsNullOrEmpty(TituloSistema)) TituloSistema = "";
            if (String.IsNullOrEmpty(PiePagina)) PiePagina = "{0} {1} {2}";
            if (String.IsNullOrEmpty(NombreCompleto)) NombreCompleto = "";
            PdfHtml footerHtml = new PdfHtml(5, 0,
                    String.Format(PiePagina, TituloSistema, NombreCompleto, DateTime.Now), null);
            footerHtml.FitDestHeight = true;
            htmlToPdfDocument.Footer.Layout(footerHtml);

            // add page numbering
            System.Drawing.Font pageNumberingFont =
                            new System.Drawing.Font(
                            new System.Drawing.FontFamily("Times New Roman"),
                            7, System.Drawing.GraphicsUnit.Point);
            PdfText pageNumberingText = new PdfText(footerWidth - 100, 6,
                            "Página {CrtPage} de {PageCount}", pageNumberingFont);
            pageNumberingText.HorizontalAlign = PdfTextHAlign.Center;
            pageNumberingText.EmbedSystemFont = true;
            pageNumberingText.ForeColor = System.Drawing.Color.Gray;
            htmlToPdfDocument.Footer.Layout(pageNumberingText);
        }
        //private void SetFooterFirmantes(PdfDocumentControl htmlToPdfDocument, footer footer)
        //{
        //    // enable footer display
        //    htmlToPdfDocument.Footer.Enabled = true;

        //    // set footer height
        //    htmlToPdfDocument.Footer.Height = footer.height;
        //    // set footer background color
        //    //htmlToPdfDocument.Footer.BackgroundColor = System.Drawing.Color.WhiteSmoke;

        //    float pdfPageWidth =
        //        htmlToPdfDocument.PageOrientation == PdfPageOrientation.Portrait ?
        //                htmlToPdfDocument.PageSize.Width : htmlToPdfDocument.PageSize.Height;
        //    float headerWidth = pdfPageWidth - htmlToPdfDocument.Margins.Left
        //                - htmlToPdfDocument.Margins.Right;
        //    float headerHeight = htmlToPdfDocument.Header.Height;

        //    PdfHtml footerHtml = new PdfHtml(0,0,footer.url);
        //    footerHtml.FitDestHeight = true;
        //    htmlToPdfDocument.Footer.Layout(footerHtml);

        //    // add page numbering
        //    if (footer.paginado)
        //    {
        //        System.Drawing.Font pageNumberingFont =
        //                    new System.Drawing.Font(
        //                    new System.Drawing.FontFamily("Times New Roman"),
        //                    11, System.Drawing.GraphicsUnit.Point);
        //        PdfText pageNumberingText = new PdfText(headerWidth - 150, 3,
        //                        "Página: {CrtPage} de {PageCount}", pageNumberingFont);
        //        pageNumberingText.HorizontalAlign = PdfTextHAlign.Center;
        //        pageNumberingText.EmbedSystemFont = true;
        //        pageNumberingText.ForeColor = System.Drawing.Color.Black;
        //        htmlToPdfDocument.Footer.Layout(pageNumberingText);
        //    }
        //}

        public Int32 CountPages(String UrlFile)
        {
            try
            {
                PdfDocument document = PdfDocument.FromFile(UrlFile);
                Int32 pages = document.Pages.Count;
                document.Close();
                return pages;

            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public Int32 CountPages(Stream file)
        {
            try
            {
                PdfDocument document = PdfDocument.FromStream(file);
                Int32 pages = document.Pages.Count;
                document.Close();
                return pages;

            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        private String FormatSign(String Text)
        {
            String[] SplitText = Text.Split(' ');
            int i = 0;
            String FormatedText = String.Empty;
            foreach (String item in SplitText)
            {
                if (i == 48)
                {
                    i = 0;
                    FormatedText = FormatedText + "\r";
                }
                if (i == 47)
                    FormatedText = FormatedText + item;
                else
                    FormatedText = FormatedText + item + " ";
                i++;
            }

            return FormatedText;
        }

        private String FormatLeyenda(String Text)
        {
            String[] SplitText = Text.Split(' ');
            int i = 0;
            String FormatedText = String.Empty;
            foreach (String item in SplitText)
            {
                if (i == 20)
                {
                    i = 0;
                    FormatedText = FormatedText + "\r";
                }
                if (i == 19)
                    FormatedText = FormatedText + item;
                else
                    FormatedText = FormatedText + item + " ";
                i++;
            }

            return FormatedText;
        }

        public byte[] MergePdf(byte[] docto1, byte[] docto2)
        {
            Stream ms1 = new MemoryStream(docto1);
            Stream ms2 = new MemoryStream(docto2);

            PdfDocument final = new PdfDocument();
            final.SerialNumber = HIQPDF_SERIAL;
            
            PdfDocument archivoAuxiliar1 = PdfDocument.FromStream(ms1);
            PdfDocument archivoAuxiliar2 = PdfDocument.FromStream(ms2);
            try
            {
                        
                final.AddDocument(archivoAuxiliar1);
                final.AddDocument(archivoAuxiliar2);

                byte[] archivo = final.WriteToMemory();
                return archivo;

            }
            finally
            {
                final.Close();
                archivoAuxiliar1.Close();
                archivoAuxiliar2.Close();
               
            }

           
    
        }

        #region ItextSharp

        public byte[] PrintSignToPDF_Memoria_TextSharp(String UrlFile, String Firma, String Firmante)
        {
            MemoryStream outputPdfStream = new MemoryStream();
            PdfDocument document = PdfDocument.FromFile(UrlFile);
            document.SerialNumber = "s/va4uPX-1f/a0cHS-wcqFnYOT-gpOHk4qL-h5OAgp2C-gZ2KioqK";
            PdfFont pdfFont = document.CreateStandardFont(PdfStandardFont.Helvetica);
            byte[] pdfBuffer = document.WriteToMemory();
            var reader = new iTextSharp.text.pdf.PdfReader(pdfBuffer);
            var stamper = new iTextSharp.text.pdf.PdfStamper(reader, outputPdfStream);

            iTextSharp.text.pdf.PdfContentByte pdfContentByte2;
            iTextSharp.text.Rectangle pageSize;
            try
            {
                pdfContentByte2 = stamper.GetOverContent(1);
                pageSize = reader.GetPageSizeWithRotation(1);
                pdfContentByte2.BeginText();
            }
            catch
            {
                pdfContentByte2 = stamper.GetOverContent(2);//PafinaFirma = 2
                pageSize = reader.GetPageSizeWithRotation(1);
                pdfContentByte2.BeginText();
            }

            //Firma = String.Format("{0} {1}", Firma, Firmante);
            string[] arrays = Firma.Split(' ');
            iTextSharp.text.pdf.BaseFont baseFont = iTextSharp.text.pdf.BaseFont.CreateFont(iTextSharp.text.pdf.BaseFont.HELVETICA_BOLD, Encoding.ASCII.EncodingName, false);

            pdfContentByte2.SetFontAndSize(baseFont, 8);
            pdfContentByte2.SetRGBColorFill(192, 192, 192);


            StringBuilder bloqueFirma = new StringBuilder();
            int contador = 0;
            int saltoLineaPixeles = 0;
            for (int i = 0; i < arrays.Length; i++)
            {
                bloqueFirma.Append(arrays[i]);
                bloqueFirma.Append(" ");
                contador++;
                if ((contador == 35) || ((saltoLineaPixeles > 13) && (contador == 36)))
                {
                    pdfContentByte2.ShowTextAligned(iTextSharp.text.pdf.PdfContentByte.ALIGN_LEFT, bloqueFirma.ToString(),
                                                         pageSize.Width - (pageSize.Width / 2) - 100,
                                                        (pageSize.Height / 5) - saltoLineaPixeles - 50, 0);
                    contador = 0;
                    bloqueFirma = new StringBuilder();
                    saltoLineaPixeles = saltoLineaPixeles + 10;
                }
                else if ((i + 1) == arrays.Length)
                {
                    pdfContentByte2.ShowTextAligned(iTextSharp.text.pdf.PdfContentByte.ALIGN_LEFT, bloqueFirma.ToString(),
                                                         pageSize.Width - (pageSize.Width / 2) - 100,
                                                        (pageSize.Height / 5) - saltoLineaPixeles - 50, 0);
                    saltoLineaPixeles = saltoLineaPixeles + 10;
                }
            }

            pdfContentByte2.ShowTextAligned(iTextSharp.text.pdf.PdfContentByte.ALIGN_LEFT, Firmante.ToString(),
                                                pageSize.Width - (pageSize.Width / 2) - 100,
                                            (pageSize.Height / 5) - saltoLineaPixeles - 50, 0);

            pdfContentByte2.EndText();

            stamper.Close();

            outputPdfStream.Close();
            outputPdfStream.Dispose();
            document.Close();

            return outputPdfStream.GetBuffer();
        }

        public byte[] PrintSignToPDF_MemoriaAllPages_TextSharp(String UrlFile, String Firma, String Firmante)
        {
            MemoryStream outputPdfStream = new MemoryStream();
            PdfDocument document = PdfDocument.FromFile(UrlFile);
            document.SerialNumber = "s/va4uPX-1f/a0cHS-wcqFnYOT-gpOHk4qL-h5OAgp2C-gZ2KioqK";
            PdfFont pdfFont = document.CreateStandardFont(PdfStandardFont.Helvetica);
            byte[] pdfBuffer = document.WriteToMemory();
            var reader = new iTextSharp.text.pdf.PdfReader(pdfBuffer);
            var stamper = new iTextSharp.text.pdf.PdfStamper(reader, outputPdfStream);

            iTextSharp.text.pdf.PdfContentByte pdfContentByte2;
            iTextSharp.text.Rectangle pageSize;
            int NumOfPages = document.Pages.Count;

            for (int j = 1; j <= NumOfPages; j++)
            {

                pdfContentByte2 = stamper.GetOverContent(j);
                pageSize = reader.GetPageSizeWithRotation(j);
                pdfContentByte2.BeginText();

                //Firma = String.Format("{0} {1}", Firma, Firmante);
                string[] arrays = Firma.Split(' ');

                //iTextSharp.text.pdf.BaseFont baseFont = iTextSharp.text.pdf.BaseFont.CreateFont(iTextSharp.text.pdf.BaseFont.HELVETICA, Encoding.ASCII.EncodingName, true, true);
                iTextSharp.text.pdf.BaseFont baseFont = iTextSharp.text.pdf.BaseFont.CreateFont("c:/windows/fonts/arial.ttf", iTextSharp.text.pdf.BaseFont.IDENTITY_H, true, true);

                pdfContentByte2.SetFontAndSize(baseFont, 8);
                //pdfContentByte2.SetRGBColorFill(192, 192, 192);
                pdfContentByte2.SetRGBColorFill(0, 0, 0);


                StringBuilder bloqueFirma = new StringBuilder();
                int contador = 0;
                int saltoLineaPixeles = 0;
                for (int i = 0; i < arrays.Length; i++)
                {
                    bloqueFirma.Append(arrays[i]);
                    bloqueFirma.Append(" ");
                    contador++;
                    if ((contador == 35) || ((saltoLineaPixeles > 13) && (contador == 36)))
                    {
                        pdfContentByte2.ShowTextAligned(iTextSharp.text.pdf.PdfContentByte.ALIGN_LEFT, bloqueFirma.ToString(),
                                                             pageSize.Width - (pageSize.Width / 2) - 100,
                                                            (pageSize.Height / 5) - saltoLineaPixeles - 100, 0);
                        contador = 0;
                        bloqueFirma = new StringBuilder();
                        saltoLineaPixeles = saltoLineaPixeles + 10;
                    }
                    else if ((i + 1) == arrays.Length)
                    {
                        pdfContentByte2.ShowTextAligned(iTextSharp.text.pdf.PdfContentByte.ALIGN_LEFT, bloqueFirma.ToString(),
                                                             pageSize.Width - (pageSize.Width / 2) - 100,
                                                            (pageSize.Height / 5) - saltoLineaPixeles - 100, 0);
                        saltoLineaPixeles = saltoLineaPixeles + 10;
                    }
                }

                String[] datos = Firmante.Split('[');
                String Firmante_ = datos[0];
                String pagina = datos[1];
                //Firmante_ = Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(Firmante_)));

                pdfContentByte2.ShowTextAligned(iTextSharp.text.pdf.PdfContentByte.ALIGN_LEFT, Firmante_.ToString(),
                                                    pageSize.Width - (pageSize.Width / 2) - 100,
                                                (pageSize.Height / 5) - saltoLineaPixeles - 100, 0);

                saltoLineaPixeles = saltoLineaPixeles + 10;
                pdfContentByte2.ShowTextAligned(iTextSharp.text.pdf.PdfContentByte.ALIGN_LEFT, pagina.ToString(),
                                                    pageSize.Width - (pageSize.Width / 2) - 100,
                                                (pageSize.Height / 5) - saltoLineaPixeles - 100, 0);

                pdfContentByte2.EndText();
            }

            stamper.Close();

            outputPdfStream.Close();
            outputPdfStream.Dispose();
            document.Close();

            return outputPdfStream.GetBuffer();
        }

        public byte[] PrintSignToPDF_MemoriaAllPages_TextSharp(Stream File, String Firma, String Firmante)
        {
            MemoryStream outputPdfStream = new MemoryStream();
            PdfDocument document = PdfDocument.FromStream(File);
            document.SerialNumber = "s/va4uPX-1f/a0cHS-wcqFnYOT-gpOHk4qL-h5OAgp2C-gZ2KioqK";
            PdfFont pdfFont = document.CreateStandardFont(PdfStandardFont.Helvetica);
            byte[] pdfBuffer = document.WriteToMemory();
            var reader = new iTextSharp.text.pdf.PdfReader(pdfBuffer);
            var stamper = new iTextSharp.text.pdf.PdfStamper(reader, outputPdfStream);

            iTextSharp.text.pdf.PdfContentByte pdfContentByte2;
            iTextSharp.text.Rectangle pageSize;
            int NumOfPages = document.Pages.Count;

            for (int j = 1; j <= NumOfPages; j++)
            {

                pdfContentByte2 = stamper.GetOverContent(j);
                pageSize = reader.GetPageSizeWithRotation(j);
                pdfContentByte2.BeginText();

                //Firma = String.Format("{0} {1}", Firma, Firmante);
                string[] arrays = Firma.Split(' ');

                //iTextSharp.text.pdf.BaseFont baseFont = iTextSharp.text.pdf.BaseFont.CreateFont(iTextSharp.text.pdf.BaseFont.HELVETICA, Encoding.ASCII.EncodingName, true, true);
                iTextSharp.text.pdf.BaseFont baseFont = iTextSharp.text.pdf.BaseFont.CreateFont("c:/windows/fonts/arial.ttf", iTextSharp.text.pdf.BaseFont.IDENTITY_H, true, true);

                pdfContentByte2.SetFontAndSize(baseFont, 8);
                //pdfContentByte2.SetRGBColorFill(192, 192, 192);
                pdfContentByte2.SetRGBColorFill(0, 0, 0);


                StringBuilder bloqueFirma = new StringBuilder();
                int contador = 0;
                int saltoLineaPixeles = 0;
                for (int i = 0; i < arrays.Length; i++)
                {
                    bloqueFirma.Append(arrays[i]);
                    bloqueFirma.Append(" ");
                    contador++;
                    if ((contador == 35) || ((saltoLineaPixeles > 13) && (contador == 36)))
                    {
                        pdfContentByte2.ShowTextAligned(iTextSharp.text.pdf.PdfContentByte.ALIGN_LEFT, bloqueFirma.ToString(),
                                                             pageSize.Width - (pageSize.Width / 2) - 100,
                                                            (pageSize.Height / 5) - saltoLineaPixeles - 100, 0);
                        contador = 0;
                        bloqueFirma = new StringBuilder();
                        saltoLineaPixeles = saltoLineaPixeles + 10;
                    }
                    else if ((i + 1) == arrays.Length)
                    {
                        pdfContentByte2.ShowTextAligned(iTextSharp.text.pdf.PdfContentByte.ALIGN_LEFT, bloqueFirma.ToString(),
                                                             pageSize.Width - (pageSize.Width / 2) - 100,
                                                            (pageSize.Height / 5) - saltoLineaPixeles - 100, 0);
                        saltoLineaPixeles = saltoLineaPixeles + 10;
                    }
                }

                String[] datos = Firmante.Split('[');
                String Firmante_ = datos[0];
                String pagina = datos[1];
                //Firmante_ = Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(Firmante_)));

                pdfContentByte2.ShowTextAligned(iTextSharp.text.pdf.PdfContentByte.ALIGN_LEFT, Firmante_.ToString(),
                                                    pageSize.Width - (pageSize.Width / 2) - 100,
                                                (pageSize.Height / 5) - saltoLineaPixeles - 100, 0);

                saltoLineaPixeles = saltoLineaPixeles + 10;
                pdfContentByte2.ShowTextAligned(iTextSharp.text.pdf.PdfContentByte.ALIGN_LEFT, pagina.ToString(),
                                                    pageSize.Width - (pageSize.Width / 2) - 100,
                                                (pageSize.Height / 5) - saltoLineaPixeles - 100, 0);

                pdfContentByte2.EndText();
            }

            stamper.Close();

            outputPdfStream.Close();
            outputPdfStream.Dispose();
            document.Close();

            return outputPdfStream.GetBuffer();
        }

        public byte[] PrintSignToPDF_TextSharp(String UrlFile, String Firma, String Firmante)
        {
            MemoryStream outputPdfStream = new MemoryStream();
            PdfDocument document = PdfDocument.FromFile(UrlFile);
            document.SerialNumber = "s/va4uPX-1f/a0cHS-wcqFnYOT-gpOHk4qL-h5OAgp2C-gZ2KioqK";
            PdfFont pdfFont = document.CreateStandardFont(PdfStandardFont.Helvetica);
            byte[] pdfBuffer = document.WriteToMemory();
            var reader = new iTextSharp.text.pdf.PdfReader(pdfBuffer);
            var stamper = new iTextSharp.text.pdf.PdfStamper(reader, outputPdfStream);

            iTextSharp.text.pdf.PdfContentByte pdfContentByte2;
            iTextSharp.text.Rectangle pageSize;
            try
            {
                pdfContentByte2 = stamper.GetOverContent(1);
                pageSize = reader.GetPageSizeWithRotation(1);
                pdfContentByte2.BeginText();
            }
            catch
            {
                pdfContentByte2 = stamper.GetOverContent(2);//PafinaFirma = 2
                pageSize = reader.GetPageSizeWithRotation(1);
                pdfContentByte2.BeginText();
            }

            //Firma = String.Format("{0} {1}", Firma, Firmante);
            string[] arrays = Firma.Split(' ');
            //iTextSharp.text.pdf.BaseFont baseFont = iTextSharp.text.pdf.BaseFont.CreateFont(iTextSharp.text.pdf.BaseFont.HELVETICA_BOLD, Encoding.ASCII.EncodingName, false);
            iTextSharp.text.pdf.BaseFont baseFont = iTextSharp.text.pdf.BaseFont.CreateFont("c:/windows/fonts/arial.ttf", iTextSharp.text.pdf.BaseFont.IDENTITY_H, true);

            pdfContentByte2.SetFontAndSize(baseFont, 12);
            pdfContentByte2.SetRGBColorFill(0, 0, 0);


            StringBuilder bloqueFirma = new StringBuilder();
            int contador = 0;
            int saltoLineaPixeles = 5;
            for (int i = 0; i < arrays.Length; i++)
            {
                bloqueFirma.Append(arrays[i]);
                bloqueFirma.Append(" ");
                contador++;
                if ((contador == 35) || ((saltoLineaPixeles > 13) && (contador == 36)))
                {
                    pdfContentByte2.ShowTextAligned(iTextSharp.text.pdf.PdfContentByte.ALIGN_LEFT, bloqueFirma.ToString(),
                                                         pageSize.Width - (pageSize.Width / 5),
                                                        (pageSize.Height / 3) - saltoLineaPixeles - 110, 0);
                    contador = 0;
                    bloqueFirma = new StringBuilder();
                    saltoLineaPixeles = saltoLineaPixeles + 20;
                }
                else if ((i + 1) == arrays.Length)
                {
                    pdfContentByte2.ShowTextAligned(iTextSharp.text.pdf.PdfContentByte.ALIGN_LEFT, bloqueFirma.ToString(),
                                                         pageSize.Width - (pageSize.Width / 5),
                                                        (pageSize.Height / 3) - saltoLineaPixeles - 110, 0);
                    saltoLineaPixeles = saltoLineaPixeles + 20;
                }
            }

            pdfContentByte2.ShowTextAligned(iTextSharp.text.pdf.PdfContentByte.ALIGN_LEFT, Firmante.ToString(),
                                                pageSize.Width - (pageSize.Width / 5),
                                            (pageSize.Height / 3) - saltoLineaPixeles - 110, 0);

            pdfContentByte2.EndText();

            stamper.Close();

            outputPdfStream.Close();
            outputPdfStream.Dispose();
            document.Close();

            return outputPdfStream.GetBuffer();
        }

        public byte[] PrintSignToPDF_TextSharp(Stream File, String Firma, String Firmante)
        {
            MemoryStream outputPdfStream = new MemoryStream();
            PdfDocument document = PdfDocument.FromStream(File);
            document.SerialNumber = "s/va4uPX-1f/a0cHS-wcqFnYOT-gpOHk4qL-h5OAgp2C-gZ2KioqK";
            PdfFont pdfFont = document.CreateStandardFont(PdfStandardFont.Helvetica);
            byte[] pdfBuffer = document.WriteToMemory();
            var reader = new iTextSharp.text.pdf.PdfReader(pdfBuffer);
            var stamper = new iTextSharp.text.pdf.PdfStamper(reader, outputPdfStream);

            iTextSharp.text.pdf.PdfContentByte pdfContentByte2;
            iTextSharp.text.Rectangle pageSize;
            try
            {
                pdfContentByte2 = stamper.GetOverContent(1);
                pageSize = reader.GetPageSizeWithRotation(1);
                pdfContentByte2.BeginText();
            }
            catch
            {
                pdfContentByte2 = stamper.GetOverContent(2);//PafinaFirma = 2
                pageSize = reader.GetPageSizeWithRotation(1);
                pdfContentByte2.BeginText();
            }

            //Firma = String.Format("{0} {1}", Firma, Firmante);
            string[] arrays = Firma.Split(' ');
            //iTextSharp.text.pdf.BaseFont baseFont = iTextSharp.text.pdf.BaseFont.CreateFont(iTextSharp.text.pdf.BaseFont.HELVETICA_BOLD, Encoding.ASCII.EncodingName, false);
            iTextSharp.text.pdf.BaseFont baseFont = iTextSharp.text.pdf.BaseFont.CreateFont("c:/windows/fonts/arial.ttf", iTextSharp.text.pdf.BaseFont.IDENTITY_H, true);

            pdfContentByte2.SetFontAndSize(baseFont, 12);
            pdfContentByte2.SetRGBColorFill(0, 0, 0);


            StringBuilder bloqueFirma = new StringBuilder();
            int contador = 0;
            int saltoLineaPixeles = 5;
            for (int i = 0; i < arrays.Length; i++)
            {
                bloqueFirma.Append(arrays[i]);
                bloqueFirma.Append(" ");
                contador++;
                if ((contador == 35) || ((saltoLineaPixeles > 13) && (contador == 36)))
                {
                    pdfContentByte2.ShowTextAligned(iTextSharp.text.pdf.PdfContentByte.ALIGN_LEFT, bloqueFirma.ToString(),
                                                         pageSize.Width - (pageSize.Width / 5),
                                                        (pageSize.Height / 3) - saltoLineaPixeles - 110, 0);
                    contador = 0;
                    bloqueFirma = new StringBuilder();
                    saltoLineaPixeles = saltoLineaPixeles + 20;
                }
                else if ((i + 1) == arrays.Length)
                {
                    pdfContentByte2.ShowTextAligned(iTextSharp.text.pdf.PdfContentByte.ALIGN_LEFT, bloqueFirma.ToString(),
                                                         pageSize.Width - (pageSize.Width / 5),
                                                        (pageSize.Height / 3) - saltoLineaPixeles - 110, 0);
                    saltoLineaPixeles = saltoLineaPixeles + 20;
                }
            }

            pdfContentByte2.ShowTextAligned(iTextSharp.text.pdf.PdfContentByte.ALIGN_LEFT, Firmante.ToString(),
                                                pageSize.Width - (pageSize.Width / 5),
                                            (pageSize.Height / 3) - saltoLineaPixeles - 110, 0);

            pdfContentByte2.EndText();

            stamper.Close();

            outputPdfStream.Close();
            outputPdfStream.Dispose();
            document.Close();

            return outputPdfStream.GetBuffer();
        }

        public byte[] PrintSignToPDF2_TextSharp(String UrlFile, String Firma, String Firmante, String Nota)
        {
            MemoryStream outputPdfStream = new MemoryStream();
            PdfDocument document = PdfDocument.FromFile(UrlFile);
            document.SerialNumber = "s/va4uPX-1f/a0cHS-wcqFnYOT-gpOHk4qL-h5OAgp2C-gZ2KioqK";
            PdfFont pdfFont = document.CreateStandardFont(PdfStandardFont.Helvetica);
            byte[] pdfBuffer = document.WriteToMemory();
            var reader = new iTextSharp.text.pdf.PdfReader(pdfBuffer);
            var stamper = new iTextSharp.text.pdf.PdfStamper(reader, outputPdfStream);

            iTextSharp.text.pdf.PdfContentByte pdfContentByte2;
            iTextSharp.text.Rectangle pageSize;
            try
            {
                pdfContentByte2 = stamper.GetOverContent(1);
                pageSize = reader.GetPageSizeWithRotation(1);
                pdfContentByte2.BeginText();
            }
            catch
            {
                pdfContentByte2 = stamper.GetOverContent(2);//PafinaFirma = 2
                pageSize = reader.GetPageSizeWithRotation(1);
                pdfContentByte2.BeginText();
            }

            //Firma = String.Format("{0} {1}", Firma, Firmante);
            string[] arrays = Firma.Split(' ');
            //iTextSharp.text.pdf.BaseFont baseFont = iTextSharp.text.pdf.BaseFont.CreateFont(iTextSharp.text.pdf.BaseFont.HELVETICA_BOLD, Encoding.ASCII.EncodingName, false);
            iTextSharp.text.pdf.BaseFont baseFont = iTextSharp.text.pdf.BaseFont.CreateFont("c:/windows/fonts/arial.ttf", iTextSharp.text.pdf.BaseFont.IDENTITY_H, true);

            pdfContentByte2.SetFontAndSize(baseFont, 12);
            pdfContentByte2.SetRGBColorFill(0, 0, 0);


            StringBuilder bloqueFirma = new StringBuilder();
            int contador = 0;
            int saltoLineaPixeles = 125;
            for (int i = 0; i < arrays.Length; i++)
            {
                bloqueFirma.Append(arrays[i]);
                bloqueFirma.Append(" ");
                contador++;
                if ((contador == 35) || ((saltoLineaPixeles > 13) && (contador == 36)))
                {
                    pdfContentByte2.ShowTextAligned(iTextSharp.text.pdf.PdfContentByte.ALIGN_LEFT, bloqueFirma.ToString(),
                                                         pageSize.Width - (pageSize.Width / 5),
                                                        (pageSize.Height / 3) - saltoLineaPixeles - 110, 0);
                    contador = 0;
                    bloqueFirma = new StringBuilder();
                    saltoLineaPixeles = saltoLineaPixeles + 20;
                }
                else if ((i + 1) == arrays.Length)
                {
                    pdfContentByte2.ShowTextAligned(iTextSharp.text.pdf.PdfContentByte.ALIGN_LEFT, bloqueFirma.ToString(),
                                                         pageSize.Width - (pageSize.Width / 5),
                                                        (pageSize.Height / 3) - saltoLineaPixeles - 110, 0);
                    saltoLineaPixeles = saltoLineaPixeles + 20;
                }
            }
            String[] datos = Firmante.Split('[');
            Firmante = datos[0];
            String pagina = datos[1];
            String NoLicencia = datos[2];
            pdfContentByte2.ShowTextAligned(iTextSharp.text.pdf.PdfContentByte.ALIGN_LEFT, NoLicencia,
                                                pageSize.Width - (pageSize.Width / 5),
                                            (pageSize.Height / 3) - saltoLineaPixeles - 110, 0);

            saltoLineaPixeles = saltoLineaPixeles + 30;
            pdfContentByte2.ShowTextAligned(iTextSharp.text.pdf.PdfContentByte.ALIGN_LEFT, Firmante.ToString(),
                                                pageSize.Width - (pageSize.Width / 5),
                                            (pageSize.Height / 3) - saltoLineaPixeles - 110, 0);

            saltoLineaPixeles = saltoLineaPixeles + 20;
            pdfContentByte2.ShowTextAligned(iTextSharp.text.pdf.PdfContentByte.ALIGN_LEFT, pagina,
                                                pageSize.Width - (pageSize.Width / 5),
                                            (pageSize.Height / 3) - saltoLineaPixeles - 110, 0);


            bloqueFirma = new StringBuilder();
            saltoLineaPixeles = saltoLineaPixeles + 20;
            String[] arrayNota = Nota.Split(' ');
            contador = 0;
            pdfContentByte2.SetFontAndSize(baseFont, 10);
            for (int i = 0; i < arrayNota.Length; i++)
            {
                if (i == 0)
                    bloqueFirma = bloqueFirma.Append("Nota: ");

                bloqueFirma.Append(arrayNota[i]);
                bloqueFirma.Append(" ");
                contador++;
                if ((contador == 11) || ((saltoLineaPixeles > 13) && (contador == 12)))
                {
                    pdfContentByte2.ShowTextAligned(iTextSharp.text.pdf.PdfContentByte.ALIGN_LEFT, bloqueFirma.ToString(),
                                                         pageSize.Width - (pageSize.Width / 5),
                                                        (pageSize.Height / 3) - saltoLineaPixeles - 110, 0);
                    contador = 0;
                    bloqueFirma = new StringBuilder();
                    saltoLineaPixeles = saltoLineaPixeles + 15;
                }
                else if ((i + 1) == arrayNota.Length)
                {
                    pdfContentByte2.ShowTextAligned(iTextSharp.text.pdf.PdfContentByte.ALIGN_LEFT, bloqueFirma.ToString(),
                                                         pageSize.Width - (pageSize.Width / 5),
                                                        (pageSize.Height / 3) - saltoLineaPixeles - 110, 0);
                }
            }

            pdfContentByte2.EndText();

            stamper.Close();

            outputPdfStream.Close();
            outputPdfStream.Dispose();
            document.Close();

            return outputPdfStream.GetBuffer();
        }
        
        public byte[] PrintSignToPDF2_TextSharp(Stream File, String Firma, String Firmante, String Nota)
        {
            MemoryStream outputPdfStream = new MemoryStream();
            PdfDocument document = PdfDocument.FromStream(File);
            document.SerialNumber = "s/va4uPX-1f/a0cHS-wcqFnYOT-gpOHk4qL-h5OAgp2C-gZ2KioqK";
            PdfFont pdfFont = document.CreateStandardFont(PdfStandardFont.Helvetica);
            byte[] pdfBuffer = document.WriteToMemory();
            var reader = new iTextSharp.text.pdf.PdfReader(pdfBuffer);
            var stamper = new iTextSharp.text.pdf.PdfStamper(reader, outputPdfStream);

            iTextSharp.text.pdf.PdfContentByte pdfContentByte2;
            iTextSharp.text.Rectangle pageSize;
            try
            {
                pdfContentByte2 = stamper.GetOverContent(1);
                pageSize = reader.GetPageSizeWithRotation(1);
                pdfContentByte2.BeginText();
            }
            catch
            {
                pdfContentByte2 = stamper.GetOverContent(2);//PafinaFirma = 2
                pageSize = reader.GetPageSizeWithRotation(1);
                pdfContentByte2.BeginText();
            }

            //Firma = String.Format("{0} {1}", Firma, Firmante);
            string[] arrays = Firma.Split(' ');
            //iTextSharp.text.pdf.BaseFont baseFont = iTextSharp.text.pdf.BaseFont.CreateFont(iTextSharp.text.pdf.BaseFont.HELVETICA_BOLD, Encoding.ASCII.EncodingName, false);
            iTextSharp.text.pdf.BaseFont baseFont = iTextSharp.text.pdf.BaseFont.CreateFont("c:/windows/fonts/arial.ttf", iTextSharp.text.pdf.BaseFont.IDENTITY_H, true);

            pdfContentByte2.SetFontAndSize(baseFont, 12);
            pdfContentByte2.SetRGBColorFill(0, 0, 0);


            StringBuilder bloqueFirma = new StringBuilder();
            int contador = 0;
            int saltoLineaPixeles = 125;
            for (int i = 0; i < arrays.Length; i++)
            {
                bloqueFirma.Append(arrays[i]);
                bloqueFirma.Append(" ");
                contador++;
                if ((contador == 35) || ((saltoLineaPixeles > 13) && (contador == 36)))
                {
                    pdfContentByte2.ShowTextAligned(iTextSharp.text.pdf.PdfContentByte.ALIGN_LEFT, bloqueFirma.ToString(),
                                                         pageSize.Width - (pageSize.Width / 5),
                                                        (pageSize.Height / 3) - saltoLineaPixeles - 110, 0);
                    contador = 0;
                    bloqueFirma = new StringBuilder();
                    saltoLineaPixeles = saltoLineaPixeles + 20;
                }
                else if ((i + 1) == arrays.Length)
                {
                    pdfContentByte2.ShowTextAligned(iTextSharp.text.pdf.PdfContentByte.ALIGN_LEFT, bloqueFirma.ToString(),
                                                         pageSize.Width - (pageSize.Width / 5),
                                                        (pageSize.Height / 3) - saltoLineaPixeles - 110, 0);
                    saltoLineaPixeles = saltoLineaPixeles + 20;
                }
            }
            String[] datos = Firmante.Split('[');
            Firmante = datos[0];
            String pagina = datos[1];
            String NoLicencia = datos[2];
            pdfContentByte2.ShowTextAligned(iTextSharp.text.pdf.PdfContentByte.ALIGN_LEFT, NoLicencia,
                                                pageSize.Width - (pageSize.Width / 5),
                                            (pageSize.Height / 3) - saltoLineaPixeles - 110, 0);

            saltoLineaPixeles = saltoLineaPixeles + 30;
            pdfContentByte2.ShowTextAligned(iTextSharp.text.pdf.PdfContentByte.ALIGN_LEFT, Firmante.ToString(),
                                                pageSize.Width - (pageSize.Width / 5),
                                            (pageSize.Height / 3) - saltoLineaPixeles - 110, 0);

            saltoLineaPixeles = saltoLineaPixeles + 20;
            pdfContentByte2.ShowTextAligned(iTextSharp.text.pdf.PdfContentByte.ALIGN_LEFT, pagina,
                                                pageSize.Width - (pageSize.Width / 5),
                                            (pageSize.Height / 3) - saltoLineaPixeles - 110, 0);


            bloqueFirma = new StringBuilder();
            saltoLineaPixeles = saltoLineaPixeles + 20;
            String[] arrayNota = Nota.Split(' ');
            contador = 0;
            pdfContentByte2.SetFontAndSize(baseFont, 10);
            for (int i = 0; i < arrayNota.Length; i++)
            {
                if (i == 0)
                    bloqueFirma = bloqueFirma.Append("Nota: ");

                bloqueFirma.Append(arrayNota[i]);
                bloqueFirma.Append(" ");
                contador++;
                if ((contador == 11) || ((saltoLineaPixeles > 13) && (contador == 12)))
                {
                    pdfContentByte2.ShowTextAligned(iTextSharp.text.pdf.PdfContentByte.ALIGN_LEFT, bloqueFirma.ToString(),
                                                         pageSize.Width - (pageSize.Width / 5),
                                                        (pageSize.Height / 3) - saltoLineaPixeles - 110, 0);
                    contador = 0;
                    bloqueFirma = new StringBuilder();
                    saltoLineaPixeles = saltoLineaPixeles + 15;
                }
                else if ((i + 1) == arrayNota.Length)
                {
                    pdfContentByte2.ShowTextAligned(iTextSharp.text.pdf.PdfContentByte.ALIGN_LEFT, bloqueFirma.ToString(),
                                                         pageSize.Width - (pageSize.Width / 5),
                                                        (pageSize.Height / 3) - saltoLineaPixeles - 110, 0);
                }
            }

            pdfContentByte2.EndText();

            stamper.Close();

            outputPdfStream.Close();
            outputPdfStream.Dispose();
            document.Close();

            return outputPdfStream.GetBuffer();
        }

        #endregion

    }
}