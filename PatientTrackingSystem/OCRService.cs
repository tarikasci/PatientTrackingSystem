using System;
using System.IO;
using Tesseract;
using PdfiumViewer;

public class OcrService
{
    public static string ExtractTextFromPdf(string filePath)
    {
        try
        {
            if (!File.Exists(filePath))
            {
                return $"Dosya bulunamadı: {filePath}";
            }

            string tessDataPath = Path.Combine(Directory.GetCurrentDirectory(), "tessdata");
            if (!Directory.Exists(tessDataPath))
            {
                return $"tessdata dizini bulunamadı: {tessDataPath}";
            }

            var ocrEngine = new TesseractEngine(tessDataPath, "eng", EngineMode.Default);
            var ocrText = string.Empty;

            using (var document = PdfDocument.Load(filePath))
            {
                for (int i = 0; i < document.PageCount; i++)
                {
                    using (var page = document.Render(i, 300, 300, true))
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            page.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                            memoryStream.Position = 0;

                            using (var pix = Pix.LoadFromMemory(memoryStream.ToArray()))
                            {
                                using (var ocrPage = ocrEngine.Process(pix))
                                {
                                    ocrText += ocrPage.GetText();
                                }
                            }
                        }
                    }
                }
            }

            return ocrText;
        }
        catch (Exception ex)
        {
            return "OCR işlemi sırasında bir hata oluştu: " + ex.ToString();
        }
    }
}