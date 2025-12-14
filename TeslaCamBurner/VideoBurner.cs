using System.Diagnostics;
using System.Security.Cryptography;

namespace TeslaCamBurner
{
    public class VideoBurner
    {
        // Settings
        private readonly string ffmpegPath;
        private readonly string fontPath;
        private readonly string filterPath;
        private readonly bool showLocation;

        private readonly Parser parser = new();
        private readonly DataReceivedEventHandler progressHandler;
        private readonly EventHandler ffmpegExited;
        private SeiMetadata?[] metadata = [];
        private Process? ffmpegProcess;

        private SeiMetadata.Types.Gear? lastGear;
        private SeiMetadata.Types.AutopilotState? lastAutopilot;
        private float? lastSpeed;
        private float? lastAccel;
        private float? lastSteering;
        private double? lastLat;
        private double? lastLon;
        private double? lastHeading;
        private double? lastAccelX;
        private double? lastAccelY;
        private double? lastAccelZ;
        private bool? lastLeft;
        private bool? lastRight;
        private bool? lastBrake;

        private uint frameIndex;
        private uint lastGearStart;
        private uint lastAutopilotStart;
        private uint lastSpeedStart;
        private uint lastAccelStart;
        private uint lastSteeringStart;
        private uint lastLatStart;
        private uint lastLonStart;
        private uint lastHeadingStart;
        private uint lastAccelXStart;
        private uint lastAccelYStart;
        private uint lastAccelZStart;
        private uint lastLeftStart;
        private uint lastRightStart;
        private uint lastBrakeStart;

        private Dictionary<SeiMetadata.Types.Gear, List<Tuple<uint, uint>>> gearDict = [];
        private Dictionary<SeiMetadata.Types.AutopilotState, List<Tuple<uint, uint>>> autopilotDict = [];
        private Dictionary<float, List<Tuple<uint, uint>>> speedDict = [];
        private Dictionary<float, List<Tuple<uint, uint>>> accelDict = [];
        private Dictionary<float, List<Tuple<uint, uint>>> steeringDict = [];
        private Dictionary<double, List<Tuple<uint, uint>>> latDict = [];
        private Dictionary<double, List<Tuple<uint, uint>>> lonDict = [];
        private Dictionary<double, List<Tuple<uint, uint>>> headingDict = [];
        private Dictionary<double, List<Tuple<uint, uint>>> accelXDict = [];
        private Dictionary<double, List<Tuple<uint, uint>>> accelYDict = [];
        private Dictionary<double, List<Tuple<uint, uint>>> accelZDict = [];
        private Dictionary<bool, List<Tuple<uint, uint>>> leftDict = [];
        private Dictionary<bool, List<Tuple<uint, uint>>> rightDict = [];
        private Dictionary<bool, List<Tuple<uint, uint>>> brakeDict = [];

        public VideoBurner(EventHandler ffmpegExited, DataReceivedEventHandler progressHandler, string ffmpegPath, string fontPath, bool showLocation)
        {
            this.progressHandler = progressHandler;
            this.ffmpegExited = ffmpegExited;
            this.ffmpegPath = ffmpegPath;
            this.showLocation = showLocation;
            this.fontPath = fontPath.Replace(@"\", "/").Replace(":", @"\\:");
            filterPath = Path.Combine(Path.GetTempPath(), $"TeslaCamBurner\\Filter-{RandomNumberGenerator.GetHexString(5)}.txt");
        }

        public void CreateVideo(string inputPath, string outputPath)
        {
            ResetPartials();
            frameIndex = 0;
            gearDict = [];
            autopilotDict = [];
            speedDict = [];
            accelDict = [];
            steeringDict = [];
            latDict = [];
            lonDict = [];
            headingDict = [];
            accelXDict = [];
            accelYDict = [];
            accelZDict = [];
            leftDict = [];
            rightDict = [];
            brakeDict = [];

            // Parse metadata and render video
            metadata = parser.Parse(inputPath);
            CreateFiltersFile();
            RenderVideo(inputPath, outputPath);
        }

        private void CreateFiltersFile()
        {
            // Create optimized filter dictionaries
            foreach (SeiMetadata? thisData in metadata)
            {
                if (thisData == null)
                {
                    AddToAllDicts();
                    ResetPartials();
                    frameIndex++;
                    continue;
                }

                // Gear
                if (thisData.GearState != lastGear)
                {
                    AddToGearDict();
                    lastGearStart = frameIndex;
                    lastGear = thisData.GearState;
                }

                // Autopilot
                if (thisData.AutopilotState != lastAutopilot)
                {
                    AddToAutopilotDict();
                    lastAutopilotStart = frameIndex;
                    lastAutopilot = thisData.AutopilotState;
                }

                // Speed
                if (thisData.VehicleSpeedMps != lastSpeed)
                {
                    AddToSpeedDict();
                    lastSpeedStart = frameIndex;
                    lastSpeed = thisData.VehicleSpeedMps;
                }

                // Accelerator Position
                if (thisData.AcceleratorPedalPosition != lastAccel)
                {
                    AddToAccelDict();
                    lastAccelStart = frameIndex;
                    lastAccel = thisData.AcceleratorPedalPosition;
                }

                // Steering
                if (thisData.SteeringWheelAngle != lastSteering)
                {
                    AddToSteeringDict();
                    lastSteeringStart = frameIndex;
                    lastSteering = thisData.SteeringWheelAngle;
                }

                // Latitude
                if (thisData.LatitudeDeg != lastLat)
                {
                    AddToLatDict();
                    lastLatStart = frameIndex;
                    lastLat = thisData.LatitudeDeg;
                }

                // Longitude
                if (thisData.LongitudeDeg != lastLon)
                {
                    AddToLonDict();
                    lastLonStart = frameIndex;
                    lastLon = thisData.LongitudeDeg;
                }

                // Heading
                if (thisData.HeadingDeg != lastHeading)
                {
                    AddToHeadingDict();
                    lastHeadingStart = frameIndex;
                    lastHeading = thisData.HeadingDeg;
                }

                // Acceleration - X
                if (thisData.LinearAccelerationMps2X != lastAccelX)
                {
                    AddToAccelXDict();
                    lastAccelXStart = frameIndex;
                    lastAccelX = thisData.LinearAccelerationMps2X;
                }

                // Acceleration - Y
                if (thisData.LinearAccelerationMps2Y != lastAccelY)
                {
                    AddToAccelYDict();
                    lastAccelYStart = frameIndex;
                    lastAccelY = thisData.LinearAccelerationMps2Y;
                }

                // Acceleration - Z
                if (thisData.LinearAccelerationMps2Z != lastAccelZ)
                {
                    AddToAccelZDict();
                    lastAccelZStart = frameIndex;
                    lastAccelZ = thisData.LinearAccelerationMps2Z;
                }

                // Left Turn signal
                if (thisData.BlinkerOnLeft != lastLeft)
                {
                    AddToLeftDict();
                    lastLeftStart = frameIndex;
                    lastLeft = thisData.BlinkerOnLeft;
                }

                // Right Turn signal
                if (thisData.BlinkerOnRight != lastRight)
                {
                    AddToRightDict();
                    lastRightStart = frameIndex;
                    lastRight = thisData.BlinkerOnRight;
                }

                // Brake
                if (thisData.BrakeApplied != lastBrake)
                {
                    AddToBrakeDict();
                    lastBrakeStart = frameIndex;
                    lastBrake = thisData.BrakeApplied;
                }

                frameIndex++;
            }

            // Finish dictionaries
            AddToAllDicts();

            // Create filter text
            string filter = "[0:v]pad=iw:ih+120:0:0:color=black";
            string fontInfo = $"fontfile={fontPath}:fontsize=24:fontcolor=white";

            // Gear
            foreach (KeyValuePair<SeiMetadata.Types.Gear, List<Tuple<uint, uint>>> kvp in gearDict)
            {
                filter += $",drawtext=text='Gear\\: {kvp.Key}':{fontInfo}:x=5:y=h-25:enable='{CreateEnableString(kvp.Value)}'";
            }

            // Left Turn Signal
            foreach (KeyValuePair<bool, List<Tuple<uint, uint>>> kvp in leftDict)
            {
                filter += $",drawtext=text='Left Turn\\: {kvp.Key}':{fontInfo}:x=5:y=h-60:enable='{CreateEnableString(kvp.Value)}'";
            }

            // Linear Acceleration - X
            foreach (KeyValuePair<double, List<Tuple<uint, uint>>> kvp in accelXDict)
            {
                filter += $",drawtext=text='Linear Accel (X)\\: {kvp.Key:F4} m/s²':{fontInfo}:x=5:y=h-100:enable='{CreateEnableString(kvp.Value)}'";
            }

            // Autopilot
            foreach (KeyValuePair<SeiMetadata.Types.AutopilotState, List<Tuple<uint, uint>>> kvp in autopilotDict)
            {
                filter += $",drawtext=text='Autopilot\\: {kvp.Key}':{fontInfo}:x=200:y=h-25:enable='{CreateEnableString(kvp.Value)}'";
            }

            // Right Turn Signal
            foreach (KeyValuePair<bool, List<Tuple<uint, uint>>> kvp in rightDict)
            {
                filter += $",drawtext=text='Right Turn\\: {kvp.Key}':{fontInfo}:x=200:y=h-60:enable='{CreateEnableString(kvp.Value)}'";
            }

            // Speed
            foreach (KeyValuePair<float, List<Tuple<uint, uint>>> kvp in speedDict)
            {
                filter += $",drawtext=text='Speed\\: {(kvp.Key * 2.23694):F2} MPH':{fontInfo}:x=460:y=h-25:enable='{CreateEnableString(kvp.Value)}'";
            }

            // Brake
            foreach (KeyValuePair<bool, List<Tuple<uint, uint>>> kvp in brakeDict)
            {
                filter += $",drawtext=text='Brake\\: {kvp.Key}':{fontInfo}:x=460:y=h-60:enable='{CreateEnableString(kvp.Value)}'";
            }

            // Linear Acceleration - Y
            foreach (KeyValuePair<double, List<Tuple<uint, uint>>> kvp in accelYDict)
            {
                filter += $",drawtext=text='Linear Accel (Y)\\: {kvp.Key:F4} m/s²':{fontInfo}:x=460:y=h-100:enable='{CreateEnableString(kvp.Value)}'";
            }

            // Accelerator
            foreach (KeyValuePair<float, List<Tuple<uint, uint>>> kvp in accelDict)
            {
                filter += $",drawtext=text='Accel Pos\\: {kvp.Key:F1}':{fontInfo}:x=715:y=h-25:enable='{CreateEnableString(kvp.Value)}'";
            }

            // Heading
            foreach (KeyValuePair<double, List<Tuple<uint, uint>>> kvp in headingDict)
            {
                filter += $",drawtext=text='Heading\\: {kvp.Key:F2}':{fontInfo}:x=715:y=h-60:enable='{CreateEnableString(kvp.Value)}'";
            }

            // Steering
            foreach (KeyValuePair<float, List<Tuple<uint, uint>>> kvp in steeringDict)
            {
                filter += $",drawtext=text='Steering Wheel Angle\\: {kvp.Key:F1}':{fontInfo}:x=925:y=h-60:enable='{CreateEnableString(kvp.Value)}'";
            }

            // Linear Acceleration - Z
            foreach (KeyValuePair<double, List<Tuple<uint, uint>>> kvp in accelZDict)
            {
                filter += $",drawtext=text='Linear Accel (Z)\\: {kvp.Key:F4} m/s²':{fontInfo}:x=925:y=h-100:enable='{CreateEnableString(kvp.Value)}'";
            }

            if (showLocation)
            {
                // Latitude
                foreach (KeyValuePair<double, List<Tuple<uint, uint>>> kvp in latDict)
                {
                    filter += $",drawtext=text='Lat\\: {kvp.Key:F4}':{fontInfo}:x=925:y=h-25:enable='{CreateEnableString(kvp.Value)}'";
                }

                // Longitude
                foreach (KeyValuePair<double, List<Tuple<uint, uint>>> kvp in lonDict)
                {
                    filter += $",drawtext=text='Lon\\: {kvp.Key:F4}':{fontInfo}:x=1100:y=h-25:enable='{CreateEnableString(kvp.Value)}'";
                }
            }

            Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), "TeslaCamBurner"));
            File.WriteAllText(filterPath, filter);
        }

        private void RenderVideo(string inputPath, string outputPath)
        {
            List<string> arguments =
            [
                "-hide_banner",
                "-loglevel",
                "warning",
                "-nostats",
                "-progress",
                "pipe:1",
                "-y",
                "-i",
                inputPath,
                "-/filter_complex",
                filterPath,
                outputPath
            ];

            ffmpegProcess = new()
            {
                EnableRaisingEvents = true,
                StartInfo = new ProcessStartInfo(ffmpegPath, arguments)
                {
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true
                }
            };

            ffmpegProcess.OutputDataReceived += progressHandler;
            ffmpegProcess.Exited += ffmpegExited;
            ffmpegProcess.Start();
            ffmpegProcess.BeginOutputReadLine();
        }

        private void AddToGearDict()
        {
            if (lastGear != null)
            {
                if (!gearDict.ContainsKey((SeiMetadata.Types.Gear)lastGear)) gearDict.Add((SeiMetadata.Types.Gear)lastGear, []);
                gearDict[(SeiMetadata.Types.Gear)lastGear].Add(new(lastGearStart, frameIndex - 1));
            }
        }

        private void AddToAutopilotDict()
        {
            if (lastAutopilot != null)
            {
                if (!autopilotDict.ContainsKey((SeiMetadata.Types.AutopilotState)lastAutopilot)) autopilotDict.Add((SeiMetadata.Types.AutopilotState)lastAutopilot, []);
                autopilotDict[(SeiMetadata.Types.AutopilotState)lastAutopilot].Add(new(lastAutopilotStart, frameIndex - 1));
            }
        }

        private void AddToSpeedDict()
        {
            if (lastSpeed != null)
            {
                if (!speedDict.ContainsKey((float)lastSpeed)) speedDict.Add((float)lastSpeed, []);
                speedDict[(float)lastSpeed].Add(new(lastSpeedStart, frameIndex - 1));
            }
        }

        private void AddToAccelDict()
        {
            if (lastAccel != null)
            {
                if (!accelDict.ContainsKey((float)lastAccel)) accelDict.Add((float)lastAccel, []);
                accelDict[(float)lastAccel].Add(new(lastAccelStart, frameIndex - 1));
            }
        }

        private void AddToLatDict()
        {
            if (lastLat != null)
            {
                if (!latDict.ContainsKey((double)lastLat)) latDict.Add((double)lastLat, []);
                latDict[(double)lastLat].Add(new(lastLatStart, frameIndex - 1));
            }
        }

        private void AddToLonDict()
        {
            if (lastLon != null)
            {
                if (!lonDict.ContainsKey((double)lastLon)) lonDict.Add((double)lastLon, []);
                lonDict[(double)lastLon].Add(new(lastLonStart, frameIndex - 1));
            }
        }

        private void AddToSteeringDict()
        {
            if (lastSteering != null)
            {
                if (!steeringDict.ContainsKey((float)lastSteering)) steeringDict.Add((float)lastSteering, []);
                steeringDict[(float)lastSteering].Add(new(lastSteeringStart, frameIndex - 1));
            }
        }

        private void AddToHeadingDict()
        {
            if (lastHeading != null)
            {
                if (!headingDict.ContainsKey((double)lastHeading)) headingDict.Add((double)lastHeading, []);
                headingDict[(double)lastHeading].Add(new(lastHeadingStart, frameIndex - 1));
            }
        }

        private void AddToAccelXDict()
        {
            if (lastAccelX != null)
            {
                if (!accelXDict.ContainsKey((double)lastAccelX)) accelXDict.Add((double)lastAccelX, []);
                accelXDict[(double)lastAccelX].Add(new(lastAccelXStart, frameIndex - 1));
            }
        }

        private void AddToAccelYDict()
        {
            if (lastAccelY != null)
            {
                if (!accelYDict.ContainsKey((double)lastAccelY)) accelYDict.Add((double)lastAccelY, []);
                accelYDict[(double)lastAccelY].Add(new(lastAccelYStart, frameIndex - 1));
            }
        }

        private void AddToAccelZDict()
        {
            if (lastAccelZ != null)
            {
                if (!accelZDict.ContainsKey((double)lastAccelZ)) accelZDict.Add((double)lastAccelZ, []);
                accelZDict[(double)lastAccelZ].Add(new(lastAccelZStart, frameIndex - 1));
            }
        }

        private void AddToLeftDict()
        {
            if (lastLeft != null)
            {
                if (!leftDict.ContainsKey((bool)lastLeft)) leftDict.Add((bool)lastLeft, []);
                leftDict[(bool)lastLeft].Add(new(lastLeftStart, frameIndex - 1));
            }
        }

        private void AddToRightDict()
        {
            if (lastRight != null)
            {
                if (!rightDict.ContainsKey((bool)lastRight)) rightDict.Add((bool)lastRight, []);
                rightDict[(bool)lastRight].Add(new(lastRightStart, frameIndex - 1));
            }
        }

        private void AddToBrakeDict()
        {
            if (lastBrake != null)
            {
                if (!brakeDict.ContainsKey((bool)lastBrake)) brakeDict.Add((bool)lastBrake, []);
                brakeDict[(bool)lastBrake].Add(new(lastBrakeStart, frameIndex - 1));
            }
        }

        private void AddToAllDicts()
        {
            AddToGearDict();
            AddToAutopilotDict();
            AddToSpeedDict();
            AddToAccelDict();
            AddToSteeringDict();
            AddToLatDict();
            AddToLonDict();
            AddToHeadingDict();
            AddToAccelXDict();
            AddToAccelYDict();
            AddToAccelZDict();
            AddToLeftDict();
            AddToRightDict();
            AddToBrakeDict();
        }

        private void ResetPartials()
        {
            lastGear = null;
            lastAutopilot = null;
            lastSpeed = null;
            lastAccel = null;
            lastSteering = null;
            lastLat = null;
            lastLon = null;
            lastHeading = null;
            lastAccelX = null;
            lastAccelY = null;
            lastAccelZ = null;
            lastLeft = null;
            lastRight = null;
            lastBrake = null;

            lastGearStart = 0;
            lastAutopilotStart = 0;
            lastSpeedStart = 0;
            lastAccelStart = 0;
            lastSteeringStart = 0;
            lastLatStart = 0;
            lastLonStart = 0;
            lastHeadingStart = 0;
            lastAccelXStart = 0;
            lastAccelYStart = 0;
            lastAccelZStart = 0;
            lastLeftStart = 0;
            lastRightStart = 0;
            lastBrakeStart = 0;
        }

        private static string CreateEnableString(List<Tuple<uint, uint>> pointList)
        {
            string enableText = "";
            bool multipleTimes = false;

            foreach (Tuple<uint, uint> thisPair in pointList)
            {
                if (multipleTimes) enableText += "+";
                enableText += thisPair.Item1 == thisPair.Item2 ? $"eq(n,{thisPair.Item1})" : $"between(n,{thisPair.Item1},{thisPair.Item2})";
                multipleTimes = true;
            }

            return enableText;
        }
    }
}
