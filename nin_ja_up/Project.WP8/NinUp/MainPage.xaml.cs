using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Info;
using Windows.Foundation;
using Windows.Devices.Geolocation;
using Microsoft.Phone.Tasks;
using System.Windows.Threading;
using UnityApp = UnityPlayer.UnityApp;
using UnityBridge = WinRTBridge.WinRTBridge;
using System.Windows.Media.Imaging;
using vservWindowsPhone;//Full Ads
using InMobi.WP.AdSDK;
using GoogleAds;
using System.IO;
using Windows.ApplicationModel.Store;
using Store = Windows.ApplicationModel.Store;
namespace NinUp
{
	public partial class MainPage : PhoneApplicationPage
	{
		private bool _unityStartedLoading;
		private bool _useLocation;
        private InterstitialAd interstitialAd;//admob full
        AdView bannerAd;//admob banner
        public static int isShowAds = 0;//1 = true
        public static MainPage _mainPage = null;
		// Constructor
		public MainPage()
		{
			var bridge = new UnityBridge();
			UnityApp.SetBridge(bridge);
			InitializeComponent();
			bridge.Control = DrawingSurfaceBackground;
            WP8Statics.WP8FunctionHandleSMSOpen += WP8Statics_OpenSMSHandle;
            WP8Statics.WP8FunctionHandleStopAds += WP8Statics_StopAds;
            WP8Statics.WP8FunctionHandleRateApp += WP8Statics_RateApp;
            WP8Statics.WP8FunctionHandle2FbShared += WP8Statics_FbClickHandle;
           // WP8Statics.WP8FunctionHandlePurchaseIAP += WP8Statics_PurchaseIAPHandle;
           
            MainPage._mainPage = this;
            loadGame();
            isShowAds = 1;
            if (isShowAds == 1)
                AdmobFullAdsShow();
			/*
			VservAdControl VMB = VservAdControl.Instance; //full ads
            VMB.DisplayAd("76e808a5", LayoutRoot);  //n_m_hoang
            VMB.VservAdClosed += new EventHandler(VACCallback_OnVservAdClosing);
            VMB.VservAdError += new EventHandler(VACCallback_OnVservAdNetworkError);
            VMB.VservAdNoFill += new EventHandler(VACCallback_OnVservAdNoFill);
			*/
		}
        void WP8Statics_OpenSMSHandle(object sender, EventArgs e)
        {
            String str = (string)sender;            
            string[] strs = str.Split('|');

            SmsComposeTask smsComposeTask = new SmsComposeTask();
            smsComposeTask.To = strs[0];
            smsComposeTask.Body = strs[1];

            smsComposeTask.Show();
        }
        /*
        async void WP8Statics_PurchaseIAPHandle(object sender, EventArgs e)
        {
             String str = (string)sender;
           
            string productID = "4916GameThuanViet.1stFishHunter6000";
            //string productID = "4916GameThuanViet.1stFishHunter6000";
            
            int coin = 6000;
            if(str.Equals("2"))
            {
                productID = "4916GameThuanViet.1stFishHunter14000";
                coin  = 14000;
            }
            else if(str.Equals("2"))
            {
                productID = "4916GameThuanViet.1stFishHunter35000";
                coin = 35000;
            }
             _mainPage.Dispatcher.BeginInvoke(async () =>
             {
                 try
                 {
                     string receipt = await Store.CurrentApp.RequestProductPurchaseAsync(productID, false);
                     //if (successCallback != null)
                     //{
                     //    successCallback(receipt);
                    // }
                    // UnityEngine.se
                     
                     MessageBox.Show("Purchase succeeded");
                     WP8Statics.AddCoin(coin);
                 }
                 catch (Exception ex)
                 {
                    // if (failureCallback != null)
                     //{
                     //    failureCallback(ex);
                     //}
                     MessageBox.Show("Purchase failed:\n" + ex.Message);
                 }
             });
           
        }
         * */

        void WP8Statics_StopAds(object sender, EventArgs e)
        {
            adGridAdmob.Visibility = Visibility.Collapsed;
            isShowAds = 0;
            saveGame();
        }

        void WP8Statics_RateApp(object sender, EventArgs e)
        {
            
            MarketplaceReviewTask marketplaceReviewTask = new MarketplaceReviewTask();
            marketplaceReviewTask.Show();         
        }

        private void WP8Statics_FbClickHandle(object sender, EventArgs e)
        {
            ShareLinkTask shareLinkTask = new ShareLinkTask();
            shareLinkTask.LinkUri = new Uri("http://www.windowsphone.com/s?appid=ab13d1e8-ca3c-4a3e-9ecf-7a7f2efb0350/", UriKind.Absolute);
            shareLinkTask.Message = "Fish Hunter";
            shareLinkTask.Show();
        }

        void ShowShareMediaTask(string path)
        {
            ShareMediaTask shareMediaTask = new ShareMediaTask();
            shareMediaTask.FilePath = path;
            shareMediaTask.Show();
        }
        private void WP8Statics_FbClickHandleImage(object sender, EventArgs e)
        {
            String str = (string)sender;
            str = str.Replace("/", "\\");
           // str = "C:\\Data\\Users\\Public\\Pictures\\Camera Roll\\WP_20140702_005.jpg";
            ShowShareMediaTask(str);
            
        }
        //shared media load from store
        
        private void WP8Statics_FbSharedMediaClickHandle(object sender, EventArgs e)
        {
            CameraCaptureTask cameraCaptureTask = new CameraCaptureTask();
            cameraCaptureTask.Completed += cameraCaptureTask_Completed;
            cameraCaptureTask.Show();
        }
        void cameraCaptureTask_Completed(object sender, PhotoResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                ShowShareMediaTask(e.OriginalFileName);
            }
        }


        //end shared shared media
         void VACCallback_OnVservAdClosing(object sender, EventArgs e)
        {
         
            
        }
        void VACCallback_OnVservAdNetworkError(object sender, EventArgs e)
        {
        }

        void VACCallback_OnVservAdNoFill(object sender, EventArgs e)
        {
            AdmobFullAdsShow();
        }
        void AdmobFullAdsShow()
        {
            interstitialAd = new InterstitialAd("ca-app-pub-6844968633010430/9272626707");
            AdRequest adRequest = new AdRequest();
            interstitialAd.ReceivedAd += OnAdReceived;
            //adRequest.ForceTesting = true;//here to rem
            interstitialAd.LoadAd(adRequest);
        }
        private void OnAdReceived(object sender, AdEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Ad received successfully");            
            interstitialAd.ShowAd();
        }
		private void DrawingSurfaceBackground_Loaded(object sender, RoutedEventArgs e)
		{
			if (!_unityStartedLoading)
			{
				_unityStartedLoading = true;

				UnityApp.SetLoadedCallback(() => { Dispatcher.BeginInvoke(Unity_Loaded); });
				
				int physicalWidth, physicalHeight;
				object physicalResolution;

				var content = Application.Current.Host.Content;
				var nativeWidth = (int)Math.Floor(content.ActualWidth * content.ScaleFactor / 100.0 + 0.5);
				var nativeHeight = (int)Math.Floor(content.ActualHeight * content.ScaleFactor / 100.0 + 0.5);

				if (DeviceExtendedProperties.TryGetValue("PhysicalScreenResolution", out physicalResolution))
				{
					var resolution = (System.Windows.Size) physicalResolution;

					physicalWidth = (int)resolution.Width;
					physicalHeight = (int)resolution.Height;
				}
				else
				{
					physicalWidth = nativeWidth;
					physicalHeight = nativeHeight;
				}

				UnityApp.SetNativeResolution(nativeWidth, nativeHeight);
				UnityApp.SetRenderResolution(physicalWidth, physicalHeight);
				UnityPlayer.UnityApp.SetOrientation((int)Orientation);

				DrawingSurfaceBackground.SetBackgroundContentProvider(UnityApp.GetBackgroundContentProvider());
				DrawingSurfaceBackground.SetBackgroundManipulationHandler(UnityApp.GetManipulationHandler());
                if (isShowAds == 1)
                {
                    bannerAd = new AdView
                    {
                        Format = AdFormats.Banner,
                        AdUnitID = "ca-app-pub-6844968633010430/7795893509"
                    };

                    bannerAd.FailedToReceiveAd += OnFailedToReceiveAd;
                    bannerAd.ReceivedAd += OnReceivedAd;
                    AdRequest adRequest = new AdRequest();
                    //adRequest.ForceTesting = true;//here rem here
                    adGridAdmob.Children.Add(bannerAd);
                    bannerAd.LoadAd(adRequest);//hinh nh cai nay thua. Can kiem tra lai
                   // toanstt_Refresh_admob();
                }
            }
		}
        private void OnReceivedAd(object sender, AdEventArgs e)
        {

        }
        private void OnFailedToReceiveAd(object sender, AdErrorEventArgs errorCode)
        {
             //  adGridAdmob.Visibility = Visibility.Collapsed;
            //if (isShowAds == 1)
            //    AdsManager.showAds(DrawingSurfaceBackground, AdsManager.INDEX_INMOBI);
            // else
            //    AdsManager.showAds(DrawingSurfaceBackground, AdsManager.INDEX_INNER_ACTIVE);	
        }

        void OnTimerTick1(Object sender, EventArgs args)
        {            
            AdRequest adRequest = new AdRequest();
           // adRequest.ForceTesting = true;//here rem here
            bannerAd.LoadAd(adRequest);
            
        }        
        void toanstt_Refresh_admob()
        {
            DispatcherTimer newTimer = new DispatcherTimer();
            newTimer.Interval = TimeSpan.FromSeconds(60);
            newTimer.Tick += OnTimerTick1;
            newTimer.Start();
        }
		private void Unity_Loaded()
		{
			SetupGeolocator();            
		}

		private void PhoneApplicationPage_BackKeyPress(object sender, CancelEventArgs e)
		{
			e.Cancel = UnityApp.BackButtonPressed();
		}

		private void PhoneApplicationPage_OrientationChanged(object sender, OrientationChangedEventArgs e)
		{
			UnityApp.SetOrientation((int)e.Orientation);
		}

		protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (!UnityApp.IsLocationEnabled())
                return;
            if (IsolatedStorageSettings.ApplicationSettings.Contains("LocationConsent"))
                _useLocation = (bool)IsolatedStorageSettings.ApplicationSettings["LocationConsent"];
            else
            {
                MessageBoxResult result = MessageBox.Show("Can this application use your location?",
                    "Location Services", MessageBoxButton.OKCancel);
                _useLocation = result == MessageBoxResult.OK;
                IsolatedStorageSettings.ApplicationSettings["LocationConsent"] = _useLocation;
                IsolatedStorageSettings.ApplicationSettings.Save();
            }
        }

		private void SetupGeolocator()
        {
            if (!_useLocation)
                return;

            try
            {
				UnityApp.EnableLocationService(true);
                Geolocator geolocator = new Geolocator();
				geolocator.ReportInterval = 5000;
                IAsyncOperation<Geoposition> op = geolocator.GetGeopositionAsync();
                op.Completed += (asyncInfo, asyncStatus) =>
                    {
                        if (asyncStatus == AsyncStatus.Completed)
                        {
                            Geoposition geoposition = asyncInfo.GetResults();
                            UnityApp.SetupGeolocator(geolocator, geoposition);
                        }
                        else
                            UnityApp.SetupGeolocator(null, null);
                    };
            }
            catch (Exception)
            {
                UnityApp.SetupGeolocator(null, null);
            }
        }

        public void saveGame()
        {
            //Obtain a virtual store for application
            IsolatedStorageFile fileStorage = IsolatedStorageFile.GetUserStoreForApplication();

            //Create new subdirectory
            fileStorage.CreateDirectory("save");
            //Create a new StreamWriter, to write the file to the specified location.
            StreamWriter fileWriter = new StreamWriter(new IsolatedStorageFileStream("save\\save.txt", FileMode.OpenOrCreate, fileStorage));
            //Write the contents of our TextBox to the file.
            fileWriter.WriteLine(isShowAds);
            //Close the StreamWriter.
            fileWriter.Close();
        }
      
        public void loadGame()
        {
            //Obtain a virtual store for application
            IsolatedStorageFile fileStorage = IsolatedStorageFile.GetUserStoreForApplication();
            //Create a new StreamReader
            StreamReader fileReader = null;
            try
            {
                //Read the file from the specified location.
                fileReader = new StreamReader(new IsolatedStorageFileStream("save\\save.txt", FileMode.Open, fileStorage));
                //Read the contents of the file (the only line we created).
                string textFile = fileReader.ReadLine();
                //Write the contents of the file to the TextBlock on the page.
                fileReader.Close();
                isShowAds = int.Parse(textFile);
                return;
            }
            catch
            {
                //If they click the view button first, we need to handle the fact that the file hasn't been created yet.

            }
            isShowAds = 1;
           
        }
	}
}