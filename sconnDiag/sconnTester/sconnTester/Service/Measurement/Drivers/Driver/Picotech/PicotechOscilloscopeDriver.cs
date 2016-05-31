using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using sconnTester.Model.Test;

namespace sconnTester.Service.Measurement.Drivers.Driver.Picotech
{
    public class PicotechOscilloscopeDriver : IUsbInterfaceDriver
    {
        public bool Connect()
        {
            throw new NotImplementedException();
        }

        private void Open()
        {
            

        }

        public bool Connected { get; set; }
        public int Channels { get; set; }
        public double GetValue(int channel, ElectricMeasurementType type)
        {
            throw new NotImplementedException();
        }

        public void SetValue(int channel, double value, ElectricMeasurementType type)
        {
            throw new NotImplementedException();
        }
    }



    struct ChannelSettings
    {
        public short DCcoupled;
        public Imports.Range range;
        public short enabled;
        public short[] values;
    }

    class Pwq
    {
        public Imports.PwqConditions[] conditions;
        public short nConditions;
        public Imports.ThresholdDirection direction;
        public uint lower;
        public uint upper;
        public Imports.PulseWidthType type;

        public Pwq(Imports.PwqConditions[] conditions,
            short nConditions,
            Imports.ThresholdDirection direction,
            uint lower, uint upper,
            Imports.PulseWidthType type)
        {
            this.conditions = conditions;
            this.nConditions = nConditions;
            this.direction = direction;
            this.lower = lower;
            this.upper = upper;
            this.type = type;
        }
    }



    class PS2000ConsoleExample
    {
        private readonly short _handle;
        public const int BUFFER_SIZE = 1024;
        public const int SINGLE_SCOPE = 1;
        public const int DUAL_SCOPE = 2;
        public const int QUAD_SCOPE = 4;
        public const int MAX_CHANNELS = 4;


        short _timebase = 8;
        short _oversample = 1;
        bool _hasFastStreaming = false;

        uint _totalSampleCount = 0;
        uint _nValues = 0;
        bool _autoStop;
        short _trig;
        uint _trigAt;
        bool _appBufferFull;
        public short[][] _appBuffer = new short[2][];
        private uint _OverViewBufferSize = 150000;
        private uint _MaxSamples = 1000000;

        ushort[] inputRanges = { 10, 20, 50, 100, 200, 500, 1000, 2000, 5000, 10000, 20000, 50000 };
        private ChannelSettings[] _channelSettings;
        private int _channelCount = 2;
        private Imports.Range _firstRange;
        private Imports.Range _lastRange;
        PinnedArray<short>[] Pinned = new PinnedArray<short>[4];
        private string BlockFile = "block.txt";
        private string StreamFile = "stream.txt";

        /****************************************************************************
         * StreamingCallback
         * used by data streaming collection calls, on receipt of data.
         * used to set global flags etc checked by user routines
         ****************************************************************************/
       void StreamingCallback(short overviewBuffers,
                                          short overFlow,
                                          uint triggeredAt,
                                          short triggered,
                                          short auto_stop,
                                          uint nValues)
        {
            // used for streaming
            _autoStop = auto_stop != 0;

            // flags to show if & where a trigger has occurred
            _trig = triggered;
            _trigAt = triggeredAt;
            _nValues = nValues;

            if (nValues > 0 && !_appBufferFull)
            {
                try
                {
                    for (int i = (int)_totalSampleCount; i < nValues + _totalSampleCount; i++)
                    {
                        for (int channel = 0; channel < _channelCount; channel++)
                        {
                           // _appBuffer[channel][i] = overviewBuffers[channel * 2][i - _totalSampleCount]; //Only copying max data from 
                        }
                    }
                }
                catch (Exception) //if trying to place data 
                {
                    _appBufferFull = true;
                    Console.WriteLine("Appbuffer full collection cancelled");
                }
            }
            _totalSampleCount += nValues;
        }

        /****************************************************************************
         * SetDefaults - restore default settings
         ****************************************************************************/
        void SetDefaults()
        {
            for (int i = 0; i < _channelCount; i++) // set channels to most recent settings
            {
                Imports.SetChannel(_handle, (Imports.Channel)(i),
                                   _channelSettings[i].enabled,
                                   _channelSettings[i].DCcoupled,
                                   _channelSettings[i].range);
            }

        }

        /****************************************************************************
         * adc_to_mv
         *
         * Convert an 16-bit ADC count into millivolts
         ****************************************************************************/
        int adc_to_mv(int raw, int ch)
        {
            return (raw * inputRanges[ch]) / Imports.MaxValue;
        }

        /****************************************************************************
         * mv_to_adc
         *
         * Convert a millivolt value into a 16-bit ADC count
         *
         *  (useful for setting trigger thresholds)
         ****************************************************************************/
        short mv_to_adc(short mv, short ch)
        {
            return (short)((mv * Imports.MaxValue) / inputRanges[ch]);
        }


        /****************************************************************************
         * BlockDataHandler
         * - Used by all block data routines
         * - acquires data (user sets trigger mode before calling), displays 10 items
         *   and saves all to block.txt
         * Input :
         * - text : the text to display before the display of data slice
         * - offset : the offset into the data buffer to start the display's slice.
         ****************************************************************************/
        void BlockDataHandler(string text, int offset)
        {
            int sampleCount = BUFFER_SIZE;
            short timeunit = 0;
            int timeIndisposed;



            for (int i = 0; i < _channelCount; i++)
            {
                short[] buffer = new short[sampleCount];
                Pinned[i] = new PinnedArray<short>(buffer);
            }

            /* find the maximum number of samples, the time interval (in timeUnits),
                * the most suitable time units, and the maximum _oversample at the current _timebase*/
            int timeInterval = 0;
            int maxSamples;
            while ((Imports.GetTimebase(_handle, _timebase, sampleCount, out timeInterval, out timeunit, _oversample, out maxSamples)) == 0)
            {
                Console.WriteLine("Selected timebase {0} could not be used\n", _timebase);
                _timebase++;

            }
            Console.WriteLine("Timebase: {0}\toversample:{1}\n", _timebase, _oversample);

            /* Start it collecting, then wait for completion*/

            Imports.RunBlock(_handle, sampleCount, _timebase, _oversample, out timeIndisposed);

            Console.WriteLine("Waiting for data...Press a key to abort");
            short ready = 0;
            while ((ready = Imports.Isready(_handle)) == 0 && !Console.KeyAvailable)
            {
                Thread.Sleep(1);
            }
            if (Console.KeyAvailable) Console.ReadKey(true); // clear the key


            if (ready > 0)
            {

                short overflow;

                Imports.GetValues(_handle, Pinned[0], Pinned[1], null, null, out overflow, sampleCount);

                /* Print out the first 10 readings, converting the readings to mV if required */
                Console.WriteLine(text);



                for (int ch = 0; ch < _channelCount; ch++)
                {
                    Console.Write("Channel{0}                 ", (char)('A' + ch));
                }



                for (int i = offset; i < offset + 10; i++)
                {
                    for (int ch = 0; ch < _channelCount; ch++)
                    {
                        if (_channelSettings[ch].enabled == 1)
                        {
                            Console.Write("{0,8}                 ", adc_to_mv(Pinned[ch].Target[i], (int)_channelSettings[ch].range));
                        }

                    }
                    Console.WriteLine();
                }


                sampleCount = Math.Min(sampleCount, BUFFER_SIZE);
                //TextWriter writer = new StreamWriter(BlockFile, false);
                //writer.Write("For each of the {0} Channels, results shown are....", _channelCount);
                //writer.WriteLine();
                //writer.WriteLine("Time interval Maximum Aggregated value ADC Count & mV, Minimum Aggregated value ADC Count & mV");
                //writer.WriteLine();

                //for (int i = 0; i < _channelCount; i++)
                //    writer.Write("Time  Ch  Max ADC    Max mV   Min ADC    Min mV   ");
                //writer.WriteLine();

                for (int i = 0; i < sampleCount; i++)
                {
                    for (int ch = 0; ch < _channelCount; ch++)
                    {
                     //   writer.Write("{0,5}  ", (i * timeInterval));
                        if (_channelSettings[ch].enabled == 1)
                        {
                            //writer.Write("Ch{0} {1,7}   {2,7}   ",
                            //               (char)('A' + ch),
                            //               Pinned[ch].Target[i],
                            //               adc_to_mv(Pinned[ch].Target[i],
                            //                         (int)_channelSettings[ch].range));
                        }
                    }
               //     writer.WriteLine();
                }
               // writer.Close();

            }
            else
            {
          //      Console.WriteLine("data collection aborted");
            }

            Imports.Stop(_handle);

        }

        /****************************************************************************
         *StreamDataHandler
         * - Used by all streaming data routines
         *   and saves all to stream.txt
         *   Fast Streaming requires a local buffer to hold the data
         *   while streaming can ouput to file directly
         ****************************************************************************/
        void StreamDataHandler(bool faststreaming)
        {
            //Check if fast streaming has been selected and if device is compatible
            if (_hasFastStreaming && faststreaming)
            {
               // Console.WriteLine("Fast streaming is not support on this device");
                return;
            }

            //TextWriter writer = new StreamWriter(StreamFile, false);
            //writer.Write("For each of the {0} Channels, results shown are....", _channelCount);
            //writer.WriteLine();
            //writer.WriteLine("Time interval Maximum Aggregated value ADC Count & mV, Minimum Aggregated value ADC Count & mV");
            //writer.WriteLine();

            for (int i = 0; i < _channelCount; i++)
            {
              //  writer.Write("Time  Ch  Max ADC    Max mV   Min ADC    Min mV   ");
            }

            //writer.WriteLine();

            //Console.WriteLine("Data is being collect press any key to cancel");

            if (faststreaming)
            {
                uint noOfSamplesPerAggregate = 1;
                uint sampleInterval = 1;
                short autoStop = 1;

                _totalSampleCount = 0;
                _autoStop = false;
                _trig = 0;
                _trigAt = 0;
                _appBufferFull = false;

                for (int i = 0; i < 2; i++)
                {
                    _appBuffer[i] = new short[_MaxSamples]; //Set local buffer to hold all values
                }

                Imports.ps2000_run_streaming_ns(_handle, sampleInterval, Imports.ReportedTimeUnits.MicroSeconds, _MaxSamples, autoStop,
                                                noOfSamplesPerAggregate, _OverViewBufferSize);


                while (!_autoStop && !Console.KeyAvailable && !_appBufferFull)
                {
                    Imports.ps2000_get_streaming_last_values(_handle, StreamingCallback);

                    Console.WriteLine("Collected {0,4} samples, Total = {2,5}", _nValues, _totalSampleCount);

                    if (_trig > 0)
                    {
                        Console.WriteLine("Scope triggered at {0}  index", _totalSampleCount - _nValues + _trigAt);
                    }
                }

                Imports.Stop(_handle);

                for (int i = 0; i < _totalSampleCount; i++)
                {
                    for (int channel = 0; channel < _channelCount; channel++)
                    {
                      //  writer.Write("{0,5}  ", (i * sampleInterval));

                        if (_channelSettings[channel].enabled == 1)
                        {
                            //writer.Write("Ch{0} {1,7}   {2,7}   ",
                            //               (char)('A' + channel),
                            //               _appBuffer[channel][i],
                            //               adc_to_mv(_appBuffer[channel][i],
                            //                         (int)_channelSettings[channel].range));
                        }
                    }

                  //  writer.WriteLine();
                }
            }

            else
            {
                int no_of_samples = 0;
                short overflow;
                short sampleInterval_ms = 10;
                short windowed = 0;

                for (int i = 0; i < _channelCount; i++)
                {
                    short[] buffer = new short[_MaxSamples];
                    Pinned[i] = new PinnedArray<short>(buffer);
                }

                Imports.ps2000_run_streaming(_handle, sampleInterval_ms, (int)_MaxSamples, windowed);

                while (!Console.KeyAvailable)
                {
                    no_of_samples = Imports.GetValues(_handle, Pinned[0], Pinned[1], null, null, out overflow, BUFFER_SIZE);

                    for (int i = 0; i < no_of_samples; i++)
                    {
                        for (int ch = 0; ch < _channelCount; ch++)
                        {
                          //  writer.Write("{0,5}  ", (i * sampleInterval_ms));
                            if (_channelSettings[ch].enabled == 1)
                            {
                                //writer.Write("Ch{0} {1,7}   {2,7}   ",
                                //               (char)('A' + ch),
                                //               Pinned[ch].Target[i],
                                //               adc_to_mv(Pinned[ch].Target[i],
                                //                         (int)_channelSettings[ch].range));
                            }
                        }

                      //  writer.WriteLine();
                    }
                }

                Imports.Stop(_handle);
            }

            if (Console.KeyAvailable) Console.ReadKey(true); // clear the key  


           // writer.Close(); //close writer
        }

        //private void StreamingCallback(short overviewbuffers, short overflow, uint triggeredat, short triggered, short auto_stop, uint nvalues)
        //{
        //    throw new NotImplementedException();
        //}

        /****************************************************************************
        *  WaitForKey
        *  Wait for user's keypress
        ****************************************************************************/
        private static void WaitForKey()
        {
            while (!Console.KeyAvailable) Thread.Sleep(100);
            if (Console.KeyAvailable) Console.ReadKey(true); // clear the key
        }

        /****************************************************************************
        *  SetTrigger
        *  this function sets all the required trigger parameters, and calls the 
        *  triggering functions
        ****************************************************************************/
        short SetTrigger(Imports.TriggerChannelProperties[] channelProperties,
                        short nChannelProperties,
                        Imports.TriggerConditions[] triggerConditions,
                        short nTriggerConditions,
                        Imports.ThresholdDirection[] directions,
                        Pwq pwq,
                        uint delay,
                        int autoTriggerMs)
        {
            short status;

            if ((status = Imports.SetTriggerChannelProperties(_handle, channelProperties, nChannelProperties, autoTriggerMs)) == 0)
            {
                return status;
            }

            if ((status = Imports.SetTriggerChannelConditions(_handle, triggerConditions, nTriggerConditions)) == 0)
            {
                return status;
            }

            if (directions == null) directions = new Imports.ThresholdDirection[] { Imports.ThresholdDirection.None,
        Imports.ThresholdDirection.None, Imports.ThresholdDirection.None, Imports.ThresholdDirection.None,
        Imports.ThresholdDirection.None, Imports.ThresholdDirection.None};

            if ((status = Imports.SetTriggerChannelDirections(_handle,
                                                              directions[(int)Imports.Channel.ChannelA],
                                                              directions[(int)Imports.Channel.ChannelB],
                                                              directions[(int)Imports.Channel.ChannelC],
                                                              directions[(int)Imports.Channel.ChannelD],
                                                              directions[(int)Imports.Channel.External])) == 0)
            {
                return status;
            }

            if ((status = Imports.SetTriggerDelay(_handle, delay, 0)) == 0)
            {
                return status;
            }

            if (pwq == null) pwq = new Pwq(null, 0, Imports.ThresholdDirection.None, 0, 0, Imports.PulseWidthType.None);

            status = Imports.SetPulseWidthQualifier(_handle, pwq.conditions,
                                                    pwq.nConditions, pwq.direction,
                                                    pwq.lower, pwq.upper, pwq.type);


            return status;
        }

        /****************************************************************************
        * CollectBlockImmediate
        *  this function demonstrates how to collect a single block of data
        *  from the unit (start collecting immediately)
        ****************************************************************************/
        void CollectBlockImmediate()
        {
            Console.WriteLine("Collect Block Immediate");
            Console.WriteLine("Data is written to disk file ({0})", BlockFile);
            Console.WriteLine("Press a key to start...");
            Console.WriteLine();
            WaitForKey();

            SetDefaults();

            /* Trigger disabled	*/
            SetTrigger(null, 0, null, 0, null, null, 0, 0);

            BlockDataHandler("First 10 readings", 0);
        }

        /****************************************************************************
       *  CollectBlockTriggered
       *  this function demonstrates how to collect a single block of data from the
       *  unit, when a trigger event occurs.
       ****************************************************************************/
        void CollectBlockTriggered()
        {
            short triggerVoltage = mv_to_adc(1000, (short)_channelSettings[(int)Imports.Channel.ChannelA].range); // ChannelInfo stores ADC counts
            Imports.TriggerChannelProperties[] sourceDetails = new Imports.TriggerChannelProperties[] {
                new Imports.TriggerChannelProperties(triggerVoltage,
                                             triggerVoltage,
                                             256*10,
                                             Imports.Channel.ChannelA,
                                             Imports.ThresholdMode.Level)};


            Imports.TriggerConditions[] conditions = new Imports.TriggerConditions[] {
              new Imports.TriggerConditions(Imports.TriggerState.True,                      // Channel A
                                            Imports.TriggerState.DontCare,                  // Channel B
                                            Imports.TriggerState.DontCare,                  // Channel C
                                            Imports.TriggerState.DontCare,                  // Channel C
                                            Imports.TriggerState.DontCare,                  // external
                                            Imports.TriggerState.DontCare                  // pwq
                                            )};

            Imports.ThresholdDirection[] directions = new Imports.ThresholdDirection[]
                                            { Imports.ThresholdDirection.Rising,            // Channel A
                                            Imports.ThresholdDirection.None,                // Channel B
                                            Imports.ThresholdDirection.None,                // Channel C
                                            Imports.ThresholdDirection.None,                // Channel D
                                            Imports.ThresholdDirection.None };              // ext

            Console.WriteLine("Collect Block Triggered");
            Console.WriteLine("Data is written to disk file ({0})", BlockFile);

            Console.Write("Collects when value rises past {0}mV", adc_to_mv(sourceDetails[0].ThresholdMajor,
                                    (int)_channelSettings[(int)Imports.Channel.ChannelA].range));

            Console.WriteLine("Press a key to start...");
            WaitForKey();

            SetDefaults();

            /* Trigger enabled
             * Rising edge
             * Threshold = 1000mV */
            SetTrigger(sourceDetails, 1, conditions, 1, directions, null, 0, 0);

            BlockDataHandler("Ten readings after trigger", 0);
        }

        /****************************************************************************
        *  Stream
        *  this function demonstrates how to stream data
        *  from the unit (start collecting immediately)
        ****************************************************************************/
        void Stream()
        {
            Console.WriteLine("Stream Data Immediate");
            Console.WriteLine("Data is written to disk file ({0})", StreamFile);
            Console.WriteLine("Press a key to start...");
            Console.WriteLine();
            WaitForKey();

            SetDefaults();

            /* Trigger disabled	*/
            SetTrigger(null, 0, null, 0, null, null, 0, 0);

            StreamDataHandler(false);
        }

        /****************************************************************************
       *  TriggeredFastStream
       *  this function demonstrates how to stream data from the
       *  unit, and stop after trigger has occured
       ****************************************************************************/
        void TriggeredFastStream()
        {
            short triggerVoltage = mv_to_adc(1000, (short)_channelSettings[(int)Imports.Channel.ChannelA].range); // ChannelInfo stores ADC counts
            Imports.TriggerChannelProperties[] sourceDetails = new Imports.TriggerChannelProperties[] {
                new Imports.TriggerChannelProperties(triggerVoltage,
                                             triggerVoltage,
                                             256*10,
                                             Imports.Channel.ChannelA,
                                             Imports.ThresholdMode.Level)};


            Imports.TriggerConditions[] conditions = new Imports.TriggerConditions[] {
              new Imports.TriggerConditions(Imports.TriggerState.True,                      // Channel A
                                            Imports.TriggerState.DontCare,                  // Channel B
                                            Imports.TriggerState.DontCare,                  // Channel C
                                            Imports.TriggerState.DontCare,                  // Channel C
                                            Imports.TriggerState.DontCare,                  // external
                                            Imports.TriggerState.DontCare                  // pwq
                                            )};

            Imports.ThresholdDirection[] directions = new Imports.ThresholdDirection[]
                                            { Imports.ThresholdDirection.Rising,            // Channel A
                                            Imports.ThresholdDirection.None,                // Channel B
                                            Imports.ThresholdDirection.None,                // Channel C
                                            Imports.ThresholdDirection.None,                // Channel D
                                            Imports.ThresholdDirection.None };              // ext

            Console.WriteLine("Stream Data until 1000 samples after Triggered");
            Console.WriteLine("Data is written to disk file ({0})", StreamFile);

            Console.WriteLine("Press a key to start...");
            WaitForKey();

            SetDefaults();

            /* Trigger enabled
             * Rising edge
             * Threshold = 1000mV */
            SetTrigger(sourceDetails, 1, conditions, 1, directions, null, 0, 0);

            StreamDataHandler(true);
        }

        /****************************************************************************
        *  FastStream
        *  this function demonstrates how to stream data fast
        *  from the unit (start collecting immediately)
        ****************************************************************************/
        void FastStream()
        {
            Console.WriteLine("Fast Stream Data Immediate");
            Console.WriteLine("Data is written to disk file ({0})", StreamFile);
            Console.WriteLine("Press a key to start...");
            Console.WriteLine();
            WaitForKey();

            SetDefaults();

            /* Trigger disabled	*/
            SetTrigger(null, 0, null, 0, null, null, 0, 0);

            StreamDataHandler(true);
        }
        /****************************************************************************
        * Initialise unit' structure with Variant specific defaults
        ****************************************************************************/
        void GetDeviceInfo()
        {
            _firstRange = Imports.Range.Range_50MV;
            _lastRange = Imports.Range.Range_20V;
            _channelCount = DUAL_SCOPE;

            string[] description = {
                           "Driver Version    ",
                           "USB Version       ",
                           "Hardware Version  ",
                           "Variant Info      ",
                           "Serial            ",
                           "Cal Date          ",
                           "Kernel Ver        ",
                           "Digital Hardware  ",
                           "Analogue Hardware "
                         };
            System.Text.StringBuilder line = new System.Text.StringBuilder(80);

            if (_handle >= 0)
            {

                for (short i = 0; i < 9; i++)
                {
                    Imports.GetUnitInfo(_handle, line, 80, i);
                    if (i == 3)
                    {

                        if ((_channelCount = Convert.ToInt16(line[1]) - 48) == 1)
                        {
                            _firstRange = Imports.Range.Range_100MV;
                        }
                        else if ((Convert.ToInt16(line[3]) - 48) >= 3)
                        {
                            _hasFastStreaming = true;
                        }

                    }
                    Console.WriteLine("{0}: {1}", description[i], line);
                }
            }
        }

        /****************************************************************************
         * Select input voltage ranges for channels A and B
         ****************************************************************************/
        void SetVoltages()
        {
            bool valid = false;

            /* See what ranges are available... */
            for (int i = (int)_firstRange; i <= (int)_lastRange; i++)
            {
                Console.WriteLine("{0} . {1} mV", i, inputRanges[i]);
            }

            /* Ask the user to select a range */
            Console.WriteLine("Specify voltage range ({0}..{1})", _firstRange, _lastRange);
            Console.WriteLine("99 - switches channel off");
            for (int ch = 0; ch < _channelCount; ch++)
            {
                Console.WriteLine("");
                uint range = 8;

                do
                {
                    try
                    {
                        Console.WriteLine("Channel: {0}", (char)('A' + ch));
                        range = uint.Parse(Console.ReadLine());
                        valid = true;
                    }
                    catch (FormatException)
                    {
                        valid = false;
                        Console.WriteLine("\nEnter numeric values only");
                    }

                } while ((range != 99 && (range < (uint)_firstRange || range > (uint)_lastRange) || !valid));


                if (range != 99)
                {
                    _channelSettings[ch].range = (Imports.Range)range;
                    Console.WriteLine(" = {0} mV", inputRanges[range]);
                    _channelSettings[ch].enabled = 1;
                }
                else
                {
                    Console.WriteLine("Channel Switched off");
                    _channelSettings[ch].enabled = 0;
                }
            }
            SetDefaults();  // Set defaults now, so that if all but 1 channels get switched off, timebase updates to timebase 0 will work
        }

        /****************************************************************************
         *
         * Select _timebase, set _oversample to on and time units as nano seconds
         *
         ****************************************************************************/
        void SetTimebase()
        {
            int timeInterval;
            int maxSamples;
            short timeunit;
            bool valid = false;

            Console.WriteLine("Specify timebase");

            do
            {
                try
                {
                    _timebase = short.Parse(Console.ReadLine());
                    valid = true;
                }
                catch (FormatException)
                {
                    valid = false;
                    Console.WriteLine("\nEnter numeric values only");
                }

            } while (!valid);

            while ((Imports.GetTimebase(_handle, _timebase, BUFFER_SIZE, out timeInterval, out timeunit, _oversample, out maxSamples)) == 0)
            {
                Console.WriteLine("Selected timebase {0} could not be used", _timebase);
                _timebase++;
            }

            Console.WriteLine("Timebase {0} - {1} ns", _timebase, timeInterval);
            _oversample = 1;
        }



        /****************************************************************************
        * DisplaySettings 
        * Displays information about the user configurable settings in this example
        ***************************************************************************/
        void DisplaySettings()
        {
            int ch;
            int voltage;

            for (ch = 0; ch < _channelCount; ch++)
            {
                if (_channelSettings[ch].enabled == 0)
                    Console.WriteLine("Channel {0} Voltage Range = Off", (char)('A' + ch));
                else
                {
                    voltage = inputRanges[(int)_channelSettings[ch].range];
                    Console.Write("Channel {0} Voltage Range = ", (char)('A' + ch));

                    if (voltage < 1000)
                        Console.WriteLine("{0}mV", voltage);
                    else
                        Console.WriteLine("{0}V", voltage / 1000);
                }
            }
            Console.WriteLine();
        }




        /****************************************************************************
         * Run - show menu and call user selected options
         ****************************************************************************/
        public void Run()
        {
            // setup devices
            GetDeviceInfo();
            _timebase = 1;

            _channelSettings = new ChannelSettings[MAX_CHANNELS];

            for (int i = 0; i < _channelCount; i++)
            {
                _channelSettings[i].enabled = 1;
                _channelSettings[i].DCcoupled = 1; //DC coupled
                _channelSettings[i].range = Imports.Range.Range_5V;
            }

            // main loop - read key and call routine
            char ch = ' ';
            while (ch != 'X')
            {
                DisplaySettings();

                Console.WriteLine("");
                Console.WriteLine("B - Immediate Block              V - Set voltages");
                Console.WriteLine("T - Triggered Block              I - Set timebase");
                Console.WriteLine("S - Streaming                    W - Triggered Fast Streaming");
                Console.WriteLine("F - Fast Streaming");
                Console.WriteLine("                                 X - exit");
                Console.WriteLine("Operation:");

                ch = char.ToUpper(Console.ReadKey(true).KeyChar);

                Console.WriteLine("\n");
                switch (ch)
                {
                    case 'B':
                        CollectBlockImmediate();
                        break;

                    case 'T':
                        CollectBlockTriggered();
                        break;

                    case 'V':
                        SetVoltages();
                        break;

                    case 'I':
                        SetTimebase();
                        break;

                    case 'S':
                        Stream();
                        break;

                    case 'F':

                        FastStream();
                        break;

                    case 'W':
                        TriggeredFastStream();
                        break;

                    case 'X':
                        /* Handled by outer loop */
                        break;

                    default:
                        Console.WriteLine("Invalid operation");
                        break;
                }
            }
        }


        private PS2000ConsoleExample(short handle)
        {
            _handle = handle;
        }

      
    }


    public class Imports
    {
        #region constants
        private const string _DRIVER_FILENAME = "ps2000.dll";

        public const int MaxValue = 32767;
        #endregion

        #region Driver enums

        public enum WaveType : int
        {
            SINE,
            SQUARE,
            TRIANGLE,
            RAMP_UP,
            RAMP_DOWN,
            SINC,
            GAUSSIAN,
            HALF_SINE,
            DC_VOLTAGE,
            MAX_WAVE_TYPES
        }

        public enum ExtraOperations : int
        {
            ES_OFF,
            WHITENOISE,
            PRBS // Pseudo-Random Bit Stream 
        }

        public enum SweepType : int
        {
            UP,
            DOWN,
            UPDOWN,
            DOWNUP,
            MAX_SWEEP_TYPES
        }

        public enum SigGenTrigType
        {
            SIGGEN_RISING,
            SIGGEN_FALLING,
            SIGGEN_GATE_HIGH,
            SIGGEN_GATE_LOW
        }

        public enum SigGenTrigSource
        {
            SIGGEN_NONE,
            SIGGEN_SCOPE_TRIG,
            SIGGEN_AUX_IN,
            SIGGEN_EXT_IN,
            SIGGEN_SOFT_TRIG
        }

        public enum Channel : short
        {
            ChannelA,
            ChannelB,
            ChannelC,
            ChannelD,
            External,
            Aux,
            None
        }

        public enum Range : short
        {
            Range_10MV,
            Range_20MV,
            Range_50MV,
            Range_100MV,
            Range_200MV,
            Range_500MV,
            Range_1V,
            Range_2V,
            Range_5V,
            Range_10V,
            Range_20V,
            Range_50V,
        }

        public enum ReportedTimeUnits : int
        {
            FemtoSeconds,
            PicoSeconds,
            NanoSeconds,
            MicroSeconds,
            MilliSeconds,
            Seconds,
        }

        public enum ThresholdMode : int
        {
            Level,
            Window
        }

        public enum PulseWidthType : int
        {
            None,
            LessThan,
            GreaterThan,
            InRange,
            OutOfRange
        }

        public enum ThresholdDirection : int
        {
            // Values for level threshold mode
            //
            Above,
            Below,
            Rising,
            Falling,
            RisingOrFalling,

            // Values for window threshold mode
            //
            Inside = Above,
            Outside = Below,
            Enter = Rising,
            Exit = Falling,
            EnterOrExit = RisingOrFalling,
            PositiveRunt = 9,
            NegativeRunt,

            None = Rising,
        }

        public enum DownSamplingMode : int
        {
            None,
            Aggregate
        }

        public enum TriggerState : int
        {
            DontCare,
            True,
            False,
        }

        public enum RatioMode : int
        {
            None,
            Aggregate,
            Average,
            Decimate
        }


        #endregion

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TriggerChannelProperties
        {
            public short ThresholdMajor;
            public short ThresholdMinor;
            public ushort Hysteresis;
            public Channel Channel;
            public ThresholdMode ThresholdMode;


            public TriggerChannelProperties(
                short thresholdMajor,
                short thresholdMinor,
                ushort hysteresis,
                Channel channel,
                ThresholdMode thresholdMode)
            {
                this.ThresholdMajor = thresholdMajor;
                this.ThresholdMinor = thresholdMinor;
                this.Hysteresis = hysteresis;
                this.Channel = channel;
                this.ThresholdMode = thresholdMode;
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TriggerConditions
        {
            public TriggerState ChannelA;
            public TriggerState ChannelB;
            public TriggerState ChannelC;
            public TriggerState ChannelD;
            public TriggerState External;
            public TriggerState Pwq;

            public TriggerConditions(
                TriggerState channelA,
                TriggerState channelB,
                TriggerState channelC,
                TriggerState channelD,
                TriggerState external,
                TriggerState pwq)
            {
                this.ChannelA = channelA;
                this.ChannelB = channelB;
                this.ChannelC = channelC;
                this.ChannelD = channelD;
                this.External = external;
                this.Pwq = pwq;
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct PwqConditions
        {
            public TriggerState ChannelA;
            public TriggerState ChannelB;
            public TriggerState ChannelC;
            public TriggerState ChannelD;
            public TriggerState External;


            public PwqConditions(
                TriggerState channelA,
                TriggerState channelB,
                TriggerState channelC,
                TriggerState channelD,
                TriggerState external)
            {
                this.ChannelA = channelA;
                this.ChannelB = channelB;
                this.ChannelC = channelC;
                this.ChannelD = channelD;
                this.External = external;
            }
        }

        #region Driver Imports

        public delegate void ps2000StreamingReady(short overviewBuffers,
                                        short overFlow,
                                        uint triggeredAt,
                                        short triggered,
                                        short auto_stop,
                                        uint nValues);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps2000_open_unit")]
        public static extern short OpenUnit();

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps2000_close_unit")]
        public static extern short CloseUnit(short handle);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps2000_run_block")]
        public static extern short RunBlock(
                                                short handle,
                                                int no_of_samples,
                                                short timebase,
                                                short oversample,
                                                out int timeIndisposedMs);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps2000_run_streaming")]
        public static extern short ps2000_run_streaming(
                                                short handle,
                                                short sample_interval_ms,
                                                int max_samples,
                                                short windowed);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps2000_run_streaming_ns")]
        public static extern short ps2000_run_streaming_ns(
                                                short handle,
                                                uint sample_interval,
                                                ReportedTimeUnits time_units,
                                                uint max_samples,
                                                short autostop,
                                                uint noOfSamplesPerAggregate,
                                                uint overview_buffer_size);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps2000_get_streaming_last_values")]
        public static extern short ps2000_get_streaming_last_values(
                                                short handle,
                                                ps2000StreamingReady lpGetOverviewBuffersMaxMin);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps2000_stop")]
        public static extern short Stop(short handle);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps2000_ready")]
        public static extern short Isready(short handle);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps2000_set_channel")]
        public static extern short SetChannel(
                                                short handle,
                                                Channel channel,
                                                short enabled,
                                                short dc,
                                                Range range);


        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps2000SetAdvTriggerChannelDirections")]
        public static extern short SetTriggerChannelDirections(
                                                short handle,
                                                ThresholdDirection channelA,
                                                ThresholdDirection channelB,
            ThresholdDirection channelC,
                                                ThresholdDirection channelD,
                                                ThresholdDirection ext);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps2000_get_timebase")]
        public static extern short GetTimebase(
                                             short handle,
                                             short timebase,
                                             int noSamples,
                                             out int timeInterval,
                                             out short time_units,
                                             short oversample,
                                             out int maxSamples);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps2000_get_values")]
        public static extern short GetValues(
                short handle,
                short[] buffer_a,
                short[] buffer_b,
                short[] buffer_c,
                short[] buffer_d,
                out short overflow,
                int no_of_values);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps2000SetPulseWidthQualifier")]
        public static extern short SetPulseWidthQualifier(
            short handle,
            PwqConditions[] conditions,
            short nConditions,
            ThresholdDirection direction,
            uint lower,
            uint upper,
            PulseWidthType type);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps2000SetAdvTriggerChannelProperties")]
        public static extern short SetTriggerChannelProperties(
            short handle,
            TriggerChannelProperties[] channelProperties,
            short nChannelProperties,
            int autoTriggerMilliseconds);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps2000SetAdvTriggerChannelConditions")]
        public static extern short SetTriggerChannelConditions(
            short handle,
            TriggerConditions[] conditions,
            short nConditions);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps2000SetAdvTriggerDelay")]
        public static extern short SetTriggerDelay(short handle, uint delay, float preTriggerDelay);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps2000_get_unit_info")]
        public static extern short GetUnitInfo(
            short handle,
            StringBuilder infoString,
            short stringLength,
            short info);

        #endregion
    }


    public class PinnedArray<T> : IDisposable
    {
        GCHandle _pinnedHandle;
        private bool _disposed;

        public PinnedArray(int arraySize) : this(new T[arraySize]) { }

        public PinnedArray(T[] array)
        {
            _pinnedHandle = GCHandle.Alloc(array, GCHandleType.Pinned);
        }

        ~PinnedArray()
        {
            Dispose(false);
        }

        public T[] Target
        {
            get { return (T[])_pinnedHandle.Target; }
        }

        public static implicit operator T[] (PinnedArray<T> a)
        {
            if (a == null)
                return null;

            return (T[])a._pinnedHandle.Target;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            _disposed = true;

            if (disposing)
            {
                // Dispose of any IDisposable members
            }

            _pinnedHandle.Free();
        }
    }


}
