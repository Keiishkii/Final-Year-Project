using System.Collections.Generic;
using System.Linq;
using LSL;
using UnityEngine;

namespace _LSL
{
    public class LSLOutputStream_FakeRealTimeEEG : LSLOutput<float>
    {
        [SerializeField] private TextAsset _csvFile;
        [SerializeField] private float sampleRate;

        private List<string> _sensorNames;
        private List<List<float>> _data;
        private int _index;


        private void Start()
        {
            OpenCSV(out _sensorNames, out _data);

            StreamInfo streamInfo = new StreamInfo(_streamName, _streamType, _sensorNames.Count, (1.0f / Time.deltaTime), channel_format_t.cf_string);
            XMLElement channels = streamInfo.desc().append_child("channels");

            foreach (var sensorName in _sensorNames)
            {
                channels.append_child("channel").append_child_value("label", sensorName);
            }

            _outlet = new StreamOutlet(streamInfo);
            _currentSample = new float[_sensorNames.Count];
        }


        private void Update()
        {
            if (_index < _data.Count)
            {
                for (int i = 0; i < _data[_index].Count; i++)
                {
                    _currentSample[i] = _data[_index][i];
                }

                _outlet.push_sample(_currentSample);
                _index++;
            }
            else if (_data.Count > 0)
            {
                _index = 0;
                for (int i = 0; i < _data[_index].Count; i++)
                {
                    _currentSample[i] = _data[_index][i];
                }

                _outlet.push_sample(_currentSample);
                _index++;
            }
        }



        private void OpenCSV(out List<string> sensorNames, out List<List<float>> data)
        {
            string fileText = _csvFile.text;
            string[] fileLines = fileText.Split('\n');

            sensorNames = fileLines[0].Split(',').ToList();

            data = new List<List<float>>();
            for (int i = 1; i < fileLines.Length - 1; i++)
            {
                string[] valuesAsString = fileLines[i].Split(',');

                data.Add(new List<float>());
                foreach (var value in valuesAsString)
                {
                    data[i - 1].Add(float.Parse(value));
                }
            }
        }
    }
}