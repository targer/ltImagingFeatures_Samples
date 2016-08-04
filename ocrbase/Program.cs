using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Leadtools.Codecs;
using Leadtools.Forms.Ocr;
using Leadtools.Forms.DocumentWriters;
using Leadtools;

namespace ocrbase
{
    class Program
    {
        static void Main(string[] args)
        {
            string licenseFilePath = @"LEADTOOLS.lic";
            string developerKey = @"mcxvXsdTqZbnbQrDM9FSk5+RAsBJLhAIot2m3qdpoDO8oK7YMWOw1z6YpXqhCnFE";
            RasterSupport.SetLicense(licenseFilePath, developerKey);

            // Assuming you added "using Leadtools.Codecs;", "using Leadtools.Forms.Ocr;" and "using Leadtools.Forms.DocumentWriters;" at the beginning of this class
            // *** Step 1: Select the engine type and create an instance of the IOcrEngine interface.

            // We will use the LEADTOOLS OCR Advantage engine and use it in the same process
            IOcrEngine ocrEngine = OcrEngineManager.CreateEngine(OcrEngineType.Professional, false);

            // *** Step 2: Startup the engine.

            // Use the default parameters
            ocrEngine.Startup(null, null, null, @"C:\LEADTOOLS 19\Bin\Common\OcrProfessionalRuntime64");

            // *** Step 3: Create an OCR document with one or more pages.

            IOcrDocument ocrDocument = ocrEngine.DocumentManager.CreateDocument();

            // Add all the pages of a multi-page TIF image to the document
            ocrDocument.Pages.AddPages(@"C:\Users\Public\Documents\LEADTOOLS Images\OCR1.tif", 1, -1, null);

            // *** Step 4: Establish zones on the page(s), either manually or automatically

            // Automatic zoning
            ocrDocument.Pages.AutoZone(null);

            // *** Step 5: (Optional) Set the active languages to be used by the OCR engine

            // Enable English and German languages
            ocrEngine.LanguageManager.EnableLanguages(new string[] { "en", "de" });

            // *** Step 6: (Optional) Set the spell checking engine

            // Enable the spell checking system
            ocrEngine.SpellCheckManager.SpellCheckEngine = OcrSpellCheckEngine.Native;

            // *** Step 7: (Optional) Set any special recognition module options

            // Change the zone method for the first zone in the first page to be Graphics so it will not be recognized
            OcrZone ocrZone = ocrDocument.Pages[0].Zones[0];
            ocrZone.ZoneType = OcrZoneType.Text;
            ocrDocument.Pages[0].Zones[0] = ocrZone;

            // *** Step 8: Recognize

            ocrDocument.Pages.Recognize(null);

            // *** Step 9: Save recognition results

            // Save the results to a PDF file
            ocrDocument.Save(@"C:\Users\Public\Documents\LEADTOOLS Images\Document.pdf", DocumentFormat.Pdf, null);
            ocrDocument.Dispose();

            // *** Step 10: Shut down the OCR engine when finished
            ocrEngine.Shutdown();
            ocrEngine.Dispose();
        }
    }
}
