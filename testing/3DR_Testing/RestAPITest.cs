using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;

using NUnit.Framework;
using vwar.service.host;
using vwarDAL;



namespace _3DR_Testing
{
    [TestFixture]
    public class RestAPITest
    {
        protected string _baseUrl;
        protected string _apiKey;
        protected System.Web.Script.Serialization.JavaScriptSerializer _serializer;

        [SetUp]
        public void SetupTest()
        {
            _baseUrl = ConfigurationManager.AppSettings["ServiceUrl"];

            _apiKey = new APIKeyManager()
                         .CreateKey(ConfigurationManager.AppSettings["_3DR_UserName"],
                                    ConfigurationManager.AppSettings["_3DR_Password"]).Key;

            _serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
        }

        [Test]
        public void TestSearch()
        {

            ContentObject co = FedoraControl.AddRandomObject();

            try
            {
                List<string> terms = new List<string> { co.Title, co.Description };
                foreach (string keyword in co.Keywords.Split(','))
                    terms.Add(keyword);

                foreach (string term in terms)
                    Assert.True(httpSearch(term, co));

                //Alter the search terms to partials
                Random r = new Random();
                for (int i = 0; i < terms.Count; i++)
                    terms[i] = terms[i].Substring(0, r.Next(terms[i].Length));

                //Test single term search, partial match
                foreach (string term in terms)
                    Assert.True(httpSearch(term, co));

                //Test multiple term search
                Assert.True(httpSearch(String.Join(",", terms), co));
            }
            finally
            {
                //delete the content object out of the system to maintain test atomicity
                new DataAccessFactory().CreateDataRepositorProxy().DeleteContentObject(co);
            }
        }

        [Test]
        public void TestMetadata()
        {
            ContentObject co = FedoraControl.AddDefaultObject();
            try
            {
                string url = String.Format(_baseUrl + "/{0}/Metadata/json?id={1}", co.PID, _apiKey);

                Metadata metadata = _serializer.Deserialize<Metadata>(getRawJSON(url));

                Assert.AreEqual(co.ArtistName, metadata.ArtistName);
                Assert.AreEqual(co.AssetType, metadata.AssetType);
                Assert.AreEqual(co.Description, metadata.Description);
                Assert.AreEqual(co.DeveloperName, metadata.DeveloperName);
                Assert.AreEqual(co.Downloads.ToString(), metadata.Downloads);
                Assert.AreEqual(co.Format, metadata.Format);
                Assert.AreEqual(co.Keywords, metadata.Keywords);
                Assert.AreEqual(co.MissingTextures, metadata.MissingTextures);
                Assert.AreEqual(co.NumPolygons.ToString(), metadata.NumPolygons);
                Assert.AreEqual(co.NumTextures.ToString(), metadata.NumTextures);
                Assert.AreEqual(co.PID, metadata.PID);
                Assert.AreEqual(co.Revision.ToString(), metadata.Revision);
                Assert.AreEqual(co.SponsorName, metadata.SponsorName);
                Assert.AreEqual(co.SupportingFiles, metadata.SupportingFiles);
                Assert.AreEqual(co.TextureReferences, metadata.TextureReferences);
                Assert.AreEqual(co.Title, metadata.Title);
                Assert.AreEqual(co.NumberOfRevisions.ToString(), metadata.TotalRevisions);
                Assert.AreEqual(co.UnitScale, metadata.UnitScale);
                Assert.AreEqual(co.UpAxis, metadata.UpAxis);
                Assert.AreEqual(co.UploadedDate.ToString(), metadata.UploadedDate);
                Assert.AreEqual(co.Views.ToString(), metadata.Views);
            }
            finally
            {
                new DataAccessFactory().CreateDataRepositorProxy().DeleteContentObject(co);
            }
        }

        [Test]
        public void TestReviews()
        {
            ContentObject co = FedoraControl.AddDefaultObject();

            try
            {
                List<vwar.service.host.Review> createdReviews = new List<vwar.service.host.Review>();
                createdReviews.Add(FedoraControl.CreateReview(co, 3, "someone@example.com", "pretty cool"));
                
                string url = String.Format(_baseUrl + "/{0}/Reviews/json?id={1}", co.PID, _apiKey);

                //Check for single review
                List<vwar.service.host.Review> retrievedReviews = _serializer.Deserialize<List<vwar.service.host.Review>>(getRawJSON(url));

                Assert.AreEqual(retrievedReviews.Count, 1);
                Assert.AreEqual(retrievedReviews[0].Rating, createdReviews[0].Rating);
                Assert.AreEqual(retrievedReviews[0].ReviewText, createdReviews[0].ReviewText);
                Assert.AreEqual(retrievedReviews[0].Submitter, createdReviews[0].Submitter);

                //Check for multiple reviews
                createdReviews.Add(FedoraControl.CreateReview(co, 2, "someone2@example.com", "not very good"));
                createdReviews.Add(FedoraControl.CreateReview(co, 4, "someone3@example.com", "very good"));

                //get the reviews again
                retrievedReviews = _serializer.Deserialize<List<vwar.service.host.Review>>(getRawJSON(url));

                bool found = false;
                //Someone please implement a Find predicate, I am lazy
                foreach (vwar.service.host.Review cr in createdReviews)
                {
                    found = false;
                    foreach (vwar.service.host.Review rr in retrievedReviews)
                    {
                        if (cr.Submitter == rr.Submitter
                            && cr.ReviewText == rr.ReviewText
                            && cr.Rating == rr.Rating)
                        {
                            found = true;
                            break;
                        }
                        
                    }

                    if (!found) break;
                }

                Assert.True(found);

            }
            finally
            {
                new DataAccessFactory().CreateDataRepositorProxy().DeleteContentObject(co);
            }
        }

        [Test]
        public void TestGetModel()
        {
            ContentObject co = FedoraControl.AddDefaultObject();
            try
            {
                string urlTemplate = _baseUrl+"/"+co.PID+"/format/{0}?ID="+_apiKey;

                //Test each of the content types
                string[] contentTypes = { "json", "dae", "fbx", "obj", "3ds", "o3dtgz"/*, "original" */};

                using (WebClient client = new WebClient())
                {
                    foreach (string type in contentTypes)
                    {
                       byte[] data = client.DownloadData(String.Format(urlTemplate, type));
                       Assert.Greater(data.Length, 0);
                    }
                }
            }
            finally
            {
                new DataAccessFactory().CreateDataRepositorProxy().DeleteContentObject(co);
            }
        }

        [Test]
        public void TestScreenshot()
        {
            string url = _baseUrl + "/{0}/Screenshot?ID=" + _apiKey,
                   screenshotPath = ConfigurationManager.AppSettings["ContentPath"] + "\\Preconverted\\screenshot.png";

            Assert.True(testGetData(url, screenshotPath));
        }

        [Test]
        public void TestSponsorLogo()
        {
            string url = _baseUrl + "/{0}/SponsorLogo?ID=" + _apiKey,
                   logoPath = ConfigurationManager.AppSettings["ContentPath"] + "\\Preconverted\\sponsorlogo.jpg";

            Assert.True(testGetData(url, logoPath));
        }

        [Test]
        public void TestDeveloperLogo()
        {
            string url = _baseUrl + "/{0}/DeveloperLogo?ID=" + _apiKey,
                   logoPath = ConfigurationManager.AppSettings["ContentPath"] + "\\Preconverted\\devlogo.jpg";

            Assert.True(testGetData(url, logoPath));
        }

        /*[Test]*/
        //TODO: Finish this test once supporting file is correctly implemented
        public void TestSupportingFile()
        {
            ContentObject co = FedoraControl.AddDefaultObject();
            string filepath = ConfigurationManager.AppSettings["ContentPath"] + "\\Preconverted\\test.o3d";

            co.AddSupportingFile(new FileStream(filepath, FileMode.Open), "test.o3d", "supports o3d viewers");
        }

        [Test]
        public void TestGetTexture()
        {
            ContentObject co = FedoraControl.AddDefaultObject();
            string url = String.Format("{0}/{1}/Textures/ak47_dif.png?ID={2}", _baseUrl, co.PID, _apiKey),
                   texPath = ConfigurationManager.AppSettings["ContentPath"] + "\\Preconverted\\ak47_dif.png";

            Assert.True(testGetData(url, texPath));
        }

        [TearDown]
        public void TeardownTest()
        {
            new APIKeyManager().DeleteKey(_apiKey);
        }

        private bool httpSearch(string term, ContentObject target)
        {
            bool found = false;
            string urlTemplate = String.Format(_baseUrl + "/Search/{0}/json?ID={1}", term, _apiKey);

            List<SearchResult> results = _serializer.Deserialize<List<SearchResult>>(getRawJSON(urlTemplate));
            foreach (SearchResult sr in results)
            {
                if (sr.PID.Equals(target.PID))
                {
                    found = true;
                    break;
                }
            }

            return found;
        }

        private string getRawJSON(string url)
        {
            HttpWebRequest wq = (HttpWebRequest)HttpWebRequest.Create(url);
            string rawResult;
            using (HttpWebResponse res = (HttpWebResponse)wq.GetResponse())
            {
                rawResult = new StreamReader(res.GetResponseStream()).ReadToEnd();
            }

            return rawResult;
        }

        private bool testGetData(string url, string path)
        {
            ContentObject co = FedoraControl.AddDefaultObject();

            try
            {
                string clientMD5 = MD5(File.ReadAllBytes(path)),
                       serverMD5 = "";

                using (WebClient client = new WebClient())
                {
                    byte[] data = client.DownloadData(String.Format(url, co.PID));
                    serverMD5 = MD5(data);
                }

                return clientMD5.Equals(serverMD5);
            }
            finally
            {
                new DataAccessFactory().CreateDataRepositorProxy().DeleteContentObject(co);
            }
        }

        private static string MD5(byte[] crypto)
        {
            MD5CryptoServiceProvider csp = new MD5CryptoServiceProvider();
            byte[] hash = csp.ComputeHash(crypto);

            return BitConverter.ToString(hash).Replace("-", "");
        }

        
    }
}
