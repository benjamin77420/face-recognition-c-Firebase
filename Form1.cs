using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Face;
using Emgu.CV.CvEnum;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp;
using FireSharp.Response;
using Newtonsoft.Json;
using System.IO;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.Threading;

namespace openCVWork_with_files_
{
    public partial class Form1 : Form
    {
        #region variables
        private Capture video_capture = null;
        private Image<Bgr, Byte> current_frame = null;
        Mat frame = new Mat();
        //seting the path to the algorithm for the face detection.
        CascadeClassifier faceCasacdeClassifier = new CascadeClassifier("Pass the location of the haarcascade_frontalface_alt.xml, better to add it the solution");
        //will store the list of workers that are stored in the data base, for more info please refer to the database region.
        List<string> workers_names = new List<string>();
        //will stor the workers ID.
        List<string> workers_ID = new List<string>();
        //will soter the list of images that were in the recoreds 
        List<Image<Gray, Byte>> workers_images = new List<Image<Gray, byte>>();
        //will be used to lable the Images that the program will train on.
        List<int> workers_lable = new List<int>();
        //the recognizer that will evaluate the feed from the camera and the images data base for a match.
        EigenFaceRecognizer recognizer;
        //conection to firebase
        IFirebaseConfig config = new FirebaseConfig()
        {
            AuthSecret = "Enter your Firebase data here",
            BasePath = "Enter your Firebase data here"
        };

        IFirebaseClient client;
        #endregion
        public Form1()
        {
            InitializeComponent();
        }
        #region video_feed
        private void get_feed()
        {
            if (video_capture != null) video_capture.Dispose();
            video_capture = new Capture();
            Application.Idle += ProcessFrame;
        }

        private void ProcessFrame(object sender, EventArgs e)
        {
            //retriving the cframe from the camera, converting it to an Image<Bgr, Byte> and resizeing it to match the feed size 
            video_capture.Retrieve(frame, 0);
            current_frame = frame.ToImage<Bgr, Byte>().Resize(video_feed.Width, video_feed.Height, Inter.Cubic);

            //convert from bgr format to gray format 
            Mat grayImage = new Mat();
            CvInvoke.CvtColor(current_frame, grayImage, ColorConversion.Bgr2Gray);
            //Enhanceing the image to get a better result
            CvInvoke.EqualizeHist(grayImage, grayImage);
            
            //creating a array of Ractangles that will border the face that was found with the algorithm
            Rectangle[] faces = faceCasacdeClassifier.DetectMultiScale(grayImage, 1.1, 3, Size.Empty, Size.Empty);
            if (faces.Length > 0)
            {
                //will go through all the found faces 
                foreach (var face in faces)
                {
                    Image<Bgr, Byte> resultImage = current_frame.Convert<Bgr, Byte>();
                    resultImage.ROI = face;
                    detected_feed.SizeMode = PictureBoxSizeMode.StretchImage;
                    detected_feed.Image = resultImage.Bitmap;

                    Image<Gray, Byte> gray_image = resultImage.Convert<Gray, Byte>().Resize(200, 200, Inter.Cubic);
                    CvInvoke.EqualizeHist(gray_image, gray_image);

                    var result = recognizer.Predict(gray_image);

                    Debug.WriteLine(result.Label + ". " + result.Distance);
                    if (result.Label != -1 && result.Distance < 5000)
                    {
                        CvInvoke.PutText(current_frame, workers_names[result.Label], new Point(face.X - 2, face.Y - 2),
                            FontFace.HersheyComplex, 1.0, new Bgr(Color.Orange).MCvScalar);
                        CvInvoke.Rectangle(current_frame, face, new Bgr(Color.Green).MCvScalar, 2);

                    }
                    else
                    {
                        CvInvoke.PutText(current_frame, "Unknown", new Point(face.X - 2, face.Y - 2),
                            FontFace.HersheyComplex, 1.0, new Bgr(Color.Orange).MCvScalar);
                        CvInvoke.Rectangle(current_frame, face, new Bgr(Color.Red).MCvScalar, 2);
                    }   
                }
            }

            video_feed.Image = current_frame.Bitmap;

            if (current_frame != null)
                current_frame.Dispose();
        }

        private void trainFaceRecognition()
        {
            int imageCount = 0;
            //treshHold can be incressed in order to lower the hardnes of the ditection, in case that the imgs that are being used are of low quality
            double treshHold = 7500;
            workers_images.Clear();
            workers_lable.Clear();
            workers_names.Clear();
            workers_ID.Clear();

            try
            {
                //get all the images that are stored in the TrainedImages fore the recognition process
                string images_path = Directory.GetCurrentDirectory() + @"\TrainedImages";
                string[] files = Directory.GetFiles(images_path, "*.jpg", SearchOption.AllDirectories);

                foreach (var file in files)
                {
                    //resizeing the file, adding it to the list of images that will be sent, extracting the name and id of the worker form the img name
                    Image<Gray, byte> trainedImage = new Image<Gray, byte>(file).Resize(200, 200, Inter.Cubic);
                    CvInvoke.EqualizeHist(trainedImage, trainedImage);
                    workers_images.Add(trainedImage);
                    workers_lable.Add(imageCount);
                    string[] name_ID = file.Split('\\').Last().Split('_');
                    workers_names.Add(name_ID[0]);
                    workers_ID.Add(name_ID[1]);
                    imageCount++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: unable to train the face detection program, please contact support!! \n"+ex.Message);
            }
            //train the recognizer in case that there where any img files in the TrainedImages folder
            if (workers_images.Count > 0)
            {
                recognizer = new EigenFaceRecognizer(imageCount, treshHold);
                recognizer.Train(workers_images.ToArray(), workers_lable.ToArray());

                Debug.WriteLine("the recognizer has rendered the databse");
            }
        }
        #endregion
        #region database
        private void fetch_db()
        {
            FirebaseResponse res = client.Get(@"Workers");
            Dictionary<string, Worker> worker_recordes = JsonConvert.DeserializeObject<Dictionary<string, Worker>>(res.Body.ToString());
            //creating a path for the data to be stored at
            string save_path = Directory.GetCurrentDirectory() + @"\TrainedImages";
            //checking if a folder with this name already exists, if no create one 
            if (!Directory.Exists(save_path))
                Directory.CreateDirectory(save_path);

            Task.Factory.StartNew(() => {
                foreach (var worker in worker_recordes) {
                    //creating a path for every indevidual worker 
                    string worker_folder = save_path + @"\" + worker.Value.ID;
                    //checking if a folder like that already 
                    if (!Directory.Exists(worker_folder))
                    {
                        //creating a folder to accommodate the worker face Images 
                        Directory.CreateDirectory(worker_folder);

                        //fetching every img that is stored in every worker that is stored in the data base inside the save path
                        base64ToImage(worker.Value.faceImg1).Resize(200, 200, Inter.Cubic).Save(worker_folder + @"\" + worker.Value.FullName + "_" + worker.Value.ID + "_1.jpg");
                        base64ToImage(worker.Value.faceImg2).Resize(200, 200, Inter.Cubic).Save(worker_folder + @"\" + worker.Value.FullName + "_" + worker.Value.ID + "_2.jpg");
                        base64ToImage(worker.Value.faceImg3).Resize(200, 200, Inter.Cubic).Save(worker_folder + @"\" + worker.Value.FullName + "_" + worker.Value.ID + "_3.jpg");
                        base64ToImage(worker.Value.faceImg4).Resize(200, 200, Inter.Cubic).Save(worker_folder + @"\" + worker.Value.FullName + "_" + worker.Value.ID + "_4.jpg");
                        base64ToImage(worker.Value.faceImg5).Resize(200, 200, Inter.Cubic).Save(worker_folder + @"\" + worker.Value.FullName + "_" + worker.Value.ID + "_5.jpg");
                        base64ToImage(worker.Value.faceImg6).Resize(200, 200, Inter.Cubic).Save(worker_folder + @"\" + worker.Value.FullName + "_" + worker.Value.ID + "_6.jpg");
                        base64ToImage(worker.Value.faceImg7).Resize(200, 200, Inter.Cubic).Save(worker_folder + @"\" + worker.Value.FullName + "_" + worker.Value.ID + "_7.jpg");
                        base64ToImage(worker.Value.faceImg8).Resize(200, 200, Inter.Cubic).Save(worker_folder + @"\" + worker.Value.FullName + "_" + worker.Value.ID + "_8.jpg");
                        base64ToImage(worker.Value.faceImg9).Resize(200, 200, Inter.Cubic).Save(worker_folder + @"\" + worker.Value.FullName + "_" + worker.Value.ID + "_9.jpg");
                        base64ToImage(worker.Value.faceImg10).Resize(200, 200, Inter.Cubic).Save(worker_folder + @"\" + worker.Value.FullName + "_" + worker.Value.ID + "_10.jpg");
                        base64ToImage(worker.Value.faceImg11).Resize(200, 200, Inter.Cubic).Save(worker_folder + @"\" + worker.Value.FullName + "_" + worker.Value.ID + "_11.jpg");
                        base64ToImage(worker.Value.faceImg12).Resize(200, 200, Inter.Cubic).Save(worker_folder + @"\" + worker.Value.FullName + "_" + worker.Value.ID + "_12.jpg");
                        base64ToImage(worker.Value.faceImg13).Resize(200, 200, Inter.Cubic).Save(worker_folder + @"\" + worker.Value.FullName + "_" + worker.Value.ID + "_13.jpg");
                        base64ToImage(worker.Value.faceImg14).Resize(200, 200, Inter.Cubic).Save(worker_folder + @"\" + worker.Value.FullName + "_" + worker.Value.ID + "_14.jpg");
                        base64ToImage(worker.Value.faceImg15).Resize(200, 200, Inter.Cubic).Save(worker_folder + @"\" + worker.Value.FullName + "_" + worker.Value.ID + "_15.jpg");
                        base64ToImage(worker.Value.faceImg16).Resize(200, 200, Inter.Cubic).Save(worker_folder + @"\" + worker.Value.FullName + "_" + worker.Value.ID + "_16.jpg");
                        base64ToImage(worker.Value.faceImg17).Resize(200, 200, Inter.Cubic).Save(worker_folder + @"\" + worker.Value.FullName + "_" + worker.Value.ID + "_17.jpg");
                        base64ToImage(worker.Value.faceImg18).Resize(200, 200, Inter.Cubic).Save(worker_folder + @"\" + worker.Value.FullName + "_" + worker.Value.ID + "_18.jpg");
                        base64ToImage(worker.Value.faceImg19).Resize(200, 200, Inter.Cubic).Save(worker_folder + @"\" + worker.Value.FullName + "_" + worker.Value.ID + "_19.jpg");
                        base64ToImage(worker.Value.faceImg20).Resize(200, 200, Inter.Cubic).Save(worker_folder + @"\" + worker.Value.FullName + "_" + worker.Value.ID + "_20.jpg");
                        base64ToImage(worker.Value.faceImg21).Resize(200, 200, Inter.Cubic).Save(worker_folder + @"\" + worker.Value.FullName + "_" + worker.Value.ID + "_21.jpg");
                        base64ToImage(worker.Value.faceImg22).Resize(200, 200, Inter.Cubic).Save(worker_folder + @"\" + worker.Value.FullName + "_" + worker.Value.ID + "_22.jpg");
                        base64ToImage(worker.Value.faceImg23).Resize(200, 200, Inter.Cubic).Save(worker_folder + @"\" + worker.Value.FullName + "_" + worker.Value.ID + "_23.jpg");
                        base64ToImage(worker.Value.faceImg24).Resize(200, 200, Inter.Cubic).Save(worker_folder + @"\" + worker.Value.FullName + "_" + worker.Value.ID + "_24.jpg");
                        base64ToImage(worker.Value.faceImg25).Resize(200, 200, Inter.Cubic).Save(worker_folder + @"\" + worker.Value.FullName + "_" + worker.Value.ID + "_25.jpg");
                        base64ToImage(worker.Value.faceImg26).Resize(200, 200, Inter.Cubic).Save(worker_folder + @"\" + worker.Value.FullName + "_" + worker.Value.ID + "_26.jpg");
                        base64ToImage(worker.Value.faceImg27).Resize(200, 200, Inter.Cubic).Save(worker_folder + @"\" + worker.Value.FullName + "_" + worker.Value.ID + "_27.jpg");
                        base64ToImage(worker.Value.faceImg28).Resize(200, 200, Inter.Cubic).Save(worker_folder + @"\" + worker.Value.FullName + "_" + worker.Value.ID + "_28.jpg");
                        base64ToImage(worker.Value.faceImg29).Resize(200, 200, Inter.Cubic).Save(worker_folder + @"\" + worker.Value.FullName + "_" + worker.Value.ID + "_29.jpg");
                        base64ToImage(worker.Value.faceImg30).Resize(200, 200, Inter.Cubic).Save(worker_folder + @"\" + worker.Value.FullName + "_" + worker.Value.ID + "_30.jpg");
                        base64ToImage(worker.Value.faceImg31).Resize(200, 200, Inter.Cubic).Save(worker_folder + @"\" + worker.Value.FullName + "_" + worker.Value.ID + "_31.jpg");
                        base64ToImage(worker.Value.faceImg32).Resize(200, 200, Inter.Cubic).Save(worker_folder + @"\" + worker.Value.FullName + "_" + worker.Value.ID + "_32.jpg");
                        base64ToImage(worker.Value.faceImg33).Resize(200, 200, Inter.Cubic).Save(worker_folder + @"\" + worker.Value.FullName + "_" + worker.Value.ID + "_33.jpg");
                        base64ToImage(worker.Value.faceImg34).Resize(200, 200, Inter.Cubic).Save(worker_folder + @"\" + worker.Value.FullName + "_" + worker.Value.ID + "_34.jpg");
                    }
                }
            });
        }

        private Image<Bgr, Byte> base64ToImage(string base64) {
            //converting the base64 string from the data bae to an image to be later stored in a folder.
            byte[] img = Convert.FromBase64String(base64);
            MemoryStream ms = new MemoryStream(img);
            Bitmap bit = new Bitmap(ms);
            Image<Bgr, Byte> face = new Image<Bgr, Byte>(bit);
            return face;
        }
        #endregion
        #region testing 
        //uploading new workers in to the firebase for testing needs
        private void upload_worker()
        {
            string images_path = Directory.GetCurrentDirectory() + @"\TrainedImages";
            string[] files = Directory.GetFiles(images_path, "*.jpg", SearchOption.AllDirectories);
            List<Image<Gray, Byte>> img_bank = new List<Image<Gray, byte>>();
            List<string> img_string = new List<string>();
            int i = 0;
            foreach (var file in files) {
                Image<Gray, Byte> img = new Image<Bgr, byte>(file).Convert<Gray, Byte>();
                img_bank.Add(img);
                detected_feed.Image = img_bank[i].Bitmap;
                MessageBox.Show("hey");
                i++;
            }
            img_string = ImageIntoBase64String(img_bank);

            Worker new_worker = new Worker()
            {
                FullName = "Benjamin Ben-David",
                ID = "77420",
                faceImg1 = img_string[0],
                faceImg2 = img_string[1],
                faceImg3 = img_string[2],
                faceImg4 = img_string[3],
                faceImg5 = img_string[4],
                faceImg6 = img_string[5],
                faceImg7 = img_string[6],
                faceImg8 = img_string[7],
                faceImg9 = img_string[8],
                faceImg10 = img_string[9],
                faceImg11 = img_string[10],
                faceImg12 = img_string[11],
                faceImg13 = img_string[12],
                faceImg14 = img_string[13],
                faceImg15 = img_string[14],
                faceImg16 = img_string[15],
                faceImg34 = img_string[34],
                faceImg17 = img_string[17],
                faceImg18 = img_string[18],
                faceImg19 = img_string[19],
                faceImg20 = img_string[20],
                faceImg21 = img_string[21],
                faceImg22 = img_string[22],
                faceImg23 = img_string[23],
                faceImg24 = img_string[24],
                faceImg25 = img_string[25],
                faceImg26 = img_string[26],
                faceImg27 = img_string[27],
                faceImg28 = img_string[28],
                faceImg29 = img_string[29],
                faceImg30 = img_string[30],
                faceImg31 = img_string[31],
                faceImg32 = img_string[32],
                faceImg33 = img_string[33]
                
            };

            var set = client.Set("Workers/" + new_worker.ID, new_worker);
        } 
    

        public List<string> ImageIntoBase64String(List<Image<Gray, Byte>> image_bank)
        {
            List<string> string_bank = new List<string>();
            foreach (Image<Gray, Byte> img in image_bank) {
                
                Bitmap bitm = img.Bitmap;
                MemoryStream ms = new MemoryStream();
                bitm.Save(ms, ImageFormat.Bmp);
                string_bank.Add(Convert.ToBase64String(ms.ToArray()));
            }
            return string_bank;
        }

        #endregion
        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                client = new FirebaseClient(config);
            }
            catch
            {
                MessageBox.Show("Unable to connect to database, please contact support.");
            }

            fetch_db();

            trainFaceRecognition();

            get_feed();  
        }
    }
}
