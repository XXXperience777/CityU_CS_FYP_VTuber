using System;
using UnityEngine;
using UnityEngine.UI;

namespace FrostweepGames.Plugins.GoogleCloud.SpeechRecognition.Examples
{
	public class SpeechToText_Example : MonoBehaviour
	{
		private GCSpeechRecognition _speechRecognition;

		private Button _startRecordButton,
					   _stopRecordButton;

		private Text _resultText;

		private void Start()
		{
			_speechRecognition = GCSpeechRecognition.Instance;
			_speechRecognition.RecognizeSuccessEvent += RecognizeSuccessEventHandler;
			_speechRecognition.RecognizeFailedEvent += RecognizeFailedEventHandler;
		
			_speechRecognition.FinishedRecordEvent += FinishedRecordEventHandler;
			_speechRecognition.StartedRecordEvent += StartedRecordEventHandler;
			_speechRecognition.RecordFailedEvent += RecordFailedEventHandler;

			_speechRecognition.BeginTalkigEvent += BeginTalkigEventHandler;
			_speechRecognition.EndTalkigEvent += EndTalkigEventHandler;

			_startRecordButton = transform.Find("CanvasSpeechToText/Start_Button").GetComponent<Button>();
			_stopRecordButton = transform.Find("CanvasSpeechToText/End_Button").GetComponent<Button>();
			_resultText = transform.Find("CanvasSpeechToText/ResultText").GetComponent<Text>();

			_startRecordButton.onClick.AddListener(StartRecordButtonOnClickHandler);
			_stopRecordButton.onClick.AddListener(StopRecordButtonOnClickHandler);
	
			_startRecordButton.interactable = true;
			_stopRecordButton.interactable = false;
		
			//RefreshMicsButtonOnClickHandler();
		}

		private void OnDestroy()
		{
			_speechRecognition.RecognizeSuccessEvent -= RecognizeSuccessEventHandler;
			_speechRecognition.RecognizeFailedEvent -= RecognizeFailedEventHandler;

			_speechRecognition.FinishedRecordEvent -= FinishedRecordEventHandler;
			_speechRecognition.StartedRecordEvent -= StartedRecordEventHandler;
			_speechRecognition.RecordFailedEvent -= RecordFailedEventHandler;

			_speechRecognition.BeginTalkigEvent += BeginTalkigEventHandler;
			_speechRecognition.EndTalkigEvent -= EndTalkigEventHandler;
		}

		private void Update()
		{
			if(_speechRecognition.IsRecording)
			{
				if (_speechRecognition.GetMaxFrame() > 0)
				{
					float max = (float)_speechRecognition.configs[_speechRecognition.currentConfigIndex].voiceDetectionThreshold;
					float current = _speechRecognition.GetLastFrame() / max;

					// if(current >= 1f)
					// {
					// 	_voiceLevelImage.fillAmount = Mathf.Lerp(_voiceLevelImage.fillAmount, Mathf.Clamp(current / 2f, 0, 1f), 30 * Time.deltaTime);
					// }
					// else
					// {
					// 	_voiceLevelImage.fillAmount = Mathf.Lerp(_voiceLevelImage.fillAmount, Mathf.Clamp(current / 2f, 0, 0.5f), 30 * Time.deltaTime);
					// }

					// _voiceLevelImage.color = current >= 1f ? Color.green : Color.red;
				}
			}
			// else
			// {
			// 	_voiceLevelImage.fillAmount = 0f;
			// }
		}



		private void StartRecordButtonOnClickHandler()
		{
			_startRecordButton.interactable = false;
			_stopRecordButton.interactable = true;
			//_detectThresholdButton.interactable = false;
			_resultText.text = string.Empty;
			//_speechRecognition.StartRecord(_voiceDetectionToggle.isOn);
		}

		private void StopRecordButtonOnClickHandler()
		{
			_stopRecordButton.interactable = false;
			_startRecordButton.interactable = true;
		//	_detectThresholdButton.interactable = true;
			_speechRecognition.StopRecord();
		}



		private void RecognizeButtonOnClickHandler()
		{
			if (_speechRecognition.LastRecordedClip == null)
			{
				_resultText.text = "<color=red>No Record found</color>";
				return;
			}

			FinishedRecordEventHandler(_speechRecognition.LastRecordedClip, _speechRecognition.LastRecordedRaw);
		}

		private void StartedRecordEventHandler()
		{
			//_speechRecognitionState.color = Color.red;
		}

		private void RecordFailedEventHandler()
		{
			//_speechRecognitionState.color = Color.yellow;
			_resultText.text = "<color=red>Start record Failed. Please check microphone device and try again.</color>";

			_stopRecordButton.interactable = false;
			_startRecordButton.interactable = true;
		}

		private void BeginTalkigEventHandler()
		{
			_resultText.text = "<color=blue>Talk Began.</color>";
		}

		private void EndTalkigEventHandler(AudioClip clip, float[] raw)
		{
			_resultText.text += "\n<color=blue>Talk Ended.</color>";

			FinishedRecordEventHandler(clip, raw);
		}

		private void FinishedRecordEventHandler(AudioClip clip, float[] raw)
		{
			// if (!_voiceDetectionToggle.isOn && _startRecordButton.interactable)
			// {
			// //	_speechRecognitionState.color = Color.yellow;
			// }

			if (clip == null) //|| !_recognizeDirectlyToggle.isOn)
				return;

			RecognitionConfig config = RecognitionConfig.GetDefault();
			//config.languageCode = ((Enumerators.LanguageCode)_languageDropdown.value).Parse();
			// config.speechContexts = new SpeechContext[]
			// {
			// 	new SpeechContext()
			// 	{
			// 		phrases = _contextPhrasesInputField.text.Replace(" ", string.Empty).Split(',')
			// 	}
			// };
			config.audioChannelCount = clip.channels;
			// configure other parameters of the config if need

			GeneralRecognitionRequest recognitionRequest = new GeneralRecognitionRequest()
			{
				audio = new RecognitionAudioContent()
				{
					content = raw.ToBase64()
				},
				//audio = new RecognitionAudioUri() // for Google Cloud Storage object
				//{
				//	uri = "gs://bucketName/object_name"
				//},
				config = config
			};

			// if (_longRunningRecognizeToggle.isOn)
			// {
			// 	_speechRecognition.LongRunningRecognize(recognitionRequest);
			// }
			// else
			// {
				_speechRecognition.Recognize(recognitionRequest);
			// }
		}


		private void RecognizeFailedEventHandler(string error)
        {
            _resultText.text = "Recognize Failed: " + error;
        }

		

		private void RecognizeSuccessEventHandler(RecognitionResponse recognitionResponse)
        {
			//_resultText.text = "Recognize Success.";
			InsertRecognitionResponseInfo(recognitionResponse);
        }

      
		private void InsertRecognitionResponseInfo(RecognitionResponse recognitionResponse)
		{
			if (recognitionResponse == null || recognitionResponse.results.Length == 0)
			{
				_resultText.text = "\nWords not detected.";
				return;
			}

			_resultText.text += recognitionResponse.results[0].alternatives[0].transcript;
//"\n" + 
			var words = recognitionResponse.results[0].alternatives[0].words;

			// if (words != null)
			// {
			// 	string times = string.Empty;

			// 	foreach (var item in recognitionResponse.results[0].alternatives[0].words)
			// 	{
			// 		times += "<color=green>" + item.word + "</color> -  start: " + item.startTime + "; end: " + item.endTime + "\n";
			// 	}

			// 	_resultText.text += "\n" + times;
			// }

			// string other = "\nDetected alternatives: ";

			// foreach (var result in recognitionResponse.results)
			// {
			// 	foreach (var alternative in result.alternatives)
			// 	{
			// 		if (recognitionResponse.results[0].alternatives[0] != alternative)
			// 		{
			// 			other += alternative.transcript + ", ";
			// 		}
			// 	}
			// }

			// _resultText.text += other;
		}
    }
}