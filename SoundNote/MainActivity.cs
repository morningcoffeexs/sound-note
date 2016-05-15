using Android.App;
using Android.Speech; //RecognizerIntent
using Android.Widget;
using Android.OS; 
using Android.Content; //Intent
using Android.Media;
using Android.Views;
using Android.Graphics;

namespace SoundNote
{
	[Activity (Label = "SoundNote", MainLauncher = true, Icon = "@mipmap/icon",
		Theme = "@style/MyCustomTheme")]
	public class MainActivity : Activity
	{

		private readonly int VOICE = 10;
		private bool isRecording;
		private TextView textBox;
		private ImageButton micButton;
		private MediaPlayer player;

		protected override void OnCreate (Bundle savedInstanceState)

		{
			base.OnCreate (savedInstanceState);

			// Fullscreen
			RequestWindowFeature(WindowFeatures.NoTitle);
			this.Window.AddFlags(WindowManagerFlags.Fullscreen);

			// Set the isRecording flag to false (not recording)
			isRecording = false;

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			// Get the resources from the layout
			micButton = FindViewById<ImageButton> (Resource.Id.micButton);
			textBox = FindViewById<TextView> (Resource.Id.textYourText);			
			Typeface tf = Typeface.CreateFromAsset(Assets,"fonts/modes.ttf"); 
			textBox.Typeface = tf;
			textBox.Text = "Click on me and say a note!";

			if (HasMic ()) {
				micButton.Click += delegate {
					micButton.SetImageResource (Resource.Drawable.idle);
					textBox.Text = "Click on me and say a note!";
					isRecording = !isRecording; // Change to opposite state 
					SetupMic ();
				};
			} 
			else 
			{
				Android.OS.Process.KillProcess (Android.OS.Process.MyPid ());
			}
				
		}

		private bool HasMic ()
		{
			string recognization = Android.Content.PM.PackageManager.FeatureMicrophone;
			if (recognization != "android.hardware.microphone") 
			{
				var alert = new AlertDialog.Builder (micButton.Context);
				alert.SetTitle ("There is no microphone to record with");
				alert.SetPositiveButton ("OK", (sender, e) => {
					micButton.Enabled = false;
					return;
				});
				alert.Show ();
				return false;
			}
			return true;
		}

		private void SetupMic ()
		{
			// Create the intent and start the activity
			var voiceIntent = new Intent(RecognizerIntent.ActionRecognizeSpeech);
			voiceIntent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);

			try 
			{
				if(player == null) player = new MediaPlayer();
				else player.Reset();
			// Put a message on the modal dialog
				voiceIntent.PutExtra (RecognizerIntent.ExtraPrompt, Application.Context.GetString (Resource.String.messageSpeakNow));

				// If there is more than 1.5s of silence, consider the speech over
				voiceIntent.PutExtra (RecognizerIntent.ExtraSpeechInputCompleteSilenceLengthMillis, 1500);
				voiceIntent.PutExtra (RecognizerIntent.ExtraSpeechInputPossiblyCompleteSilenceLengthMillis, 1500);
				voiceIntent.PutExtra (RecognizerIntent.ExtraSpeechInputMinimumLengthMillis, 15000);
				voiceIntent.PutExtra (RecognizerIntent.ExtraMaxResults, 1);

				voiceIntent.PutExtra (RecognizerIntent.ExtraLanguage, Java.Util.Locale.Default);
				StartActivityForResult (voiceIntent, VOICE);
			}
			catch (ActivityNotFoundException e)
			{
					var noSupport = new AlertDialog.Builder (micButton.Context);
					noSupport.SetTitle ("Your phone does not support Speech-to-Text");
					noSupport.SetPositiveButton ("OK", (sender, f) => {
						micButton.Enabled = false;
						return;
					});
					noSupport.Show ();
			}
		}

		protected override void OnActivityResult(int requestCode, Result resultVal, Intent data)
		{
			if (requestCode == VOICE)
			{
				if (resultVal == Result.Ok)
				{
					var matches = data.GetStringArrayListExtra(RecognizerIntent.ExtraResults);
					if (matches.Count != 0)
					{
						string textInput = matches[0];

						// limit the output to 500 characters
						if (textInput.Length > 500)
							textInput = textInput.Substring(0, 500);
						textBox.Text = "Now playing " + textInput.ToUpper ();
						PlayNote (textInput);
					}
					else
						textBox.Text = "No speech was recognised";
					// change the text back on the button
				}
			}

			base.OnActivityResult(requestCode, resultVal, data);
		}

		private void PlayNote (string textInput) 
		{
			micButton.SetImageResource (Resource.Drawable.play);
			if (textInput.ToLower() == "a" || textInput.ToLower() == "a note") 
			{
				player = MediaPlayer.Create (this, Resource.Raw.a);
				player.Start ();
			}
			else if (textInput.ToLower() == "a sharp" || textInput.ToLower() == "b flat")
			{
				player = MediaPlayer.Create (this, Resource.Raw.bb);
				player.Start ();
			}
			else if (textInput.ToLower() == "b") 
			{
				player = MediaPlayer.Create (this, Resource.Raw.b);
				player.Start ();
			}
			else if (textInput.ToLower() == "c") 
			{
				player = MediaPlayer.Create (this, Resource.Raw.c);
				player.Start ();
			}
			else if (textInput.ToLower() == "c sharp" || textInput.ToLower() == "d flat")
			{
				player = MediaPlayer.Create (this, Resource.Raw.cs);
				player.Start ();
			}
			else if (textInput.ToLower() == "d") 
			{
				player = MediaPlayer.Create (this, Resource.Raw.d);
				player.Start ();
			}
			else if (textInput.ToLower() == "d sharp" || textInput.ToLower() == "e flat")
			{
				player = MediaPlayer.Create (this, Resource.Raw.eb);
				player.Start ();
			}
			else if (textInput.ToLower() == "e")
			{
				player = MediaPlayer.Create (this, Resource.Raw.e);
				player.Start ();
			}
			else if (textInput.ToLower() == "f") 
			{
				player = MediaPlayer.Create (this, Resource.Raw.f);
				player.Start ();
			}
			else if (textInput.ToLower() == "f sharp" || textInput.ToLower() == "g flat")
			{
				player = MediaPlayer.Create (this, Resource.Raw.fs);
				player.Start ();
			}
			else if (textInput.ToLower() == "g") 
			{
				player = MediaPlayer.Create (this, Resource.Raw.g);
				player.Start ();
			}
			else if (textInput.ToLower() == "g sharp" || textInput.ToLower() == "a flat")
			{
				player = MediaPlayer.Create (this, Resource.Raw.gs);
				player.Start ();
			}
			else {
				textBox.Text = "ERROR: You did not say a note. You said " + textInput.ToUpper();	
				micButton.SetImageResource (Resource.Drawable.idle);
			}
			player.Completion += delegate {
				player.Stop();
				micButton.SetImageResource (Resource.Drawable.idle);
				textBox.Text = "Click on me and say a note!";
			};
		}
	}
}


