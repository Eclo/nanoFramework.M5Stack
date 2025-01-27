// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Iot.Device.Ws28xx.Esp32;
using nanoFramework.M5Stack;
using nanoFramework.Presentation.Media;
using System;
using System.Diagnostics;
using System.Numerics;
using System.Threading;
using Console = nanoFramework.M5Stack.Console;

Debug.WriteLine("Hello from nanoFramework!");

Fire.InitializeScreen();
Console.Clear();

// Testing colors
const int Count = 10;
var neo = Fire.LedBar;
Console.ForegroundColor = Color.White;
Console.WriteLine("All led bar White");
ColorWipe(neo, System.Drawing.Color.White, Count);
Console.ForegroundColor = Color.Red;
Console.WriteLine("All led bar Red");
ColorWipe(neo, System.Drawing.Color.Red, Count);
Console.ForegroundColor = Color.Green;
Console.WriteLine("All led bar Green");
ColorWipe(neo, System.Drawing.Color.Green, Count);
Console.ForegroundColor = Color.Blue;
Console.WriteLine("All led bar blue");
ColorWipe(neo, System.Drawing.Color.Blue, Count);
Console.ForegroundColor = Color.White;
Console.WriteLine("All led rainbow");
Rainbow(neo, Count);
neo.Image.Clear();
neo.Update();

// Test the console display
Console.Write("This is a short text. ");
Console.ForegroundColor = Color.Red;
Console.WriteLine("This one displays in red after the previous one and is already at the next line.");
Console.BackgroundColor = Color.Yellow;
Console.ForegroundColor = Color.RoyalBlue;
Console.WriteLine("And this is really ugly but it's like that");
Console.ResetColor();
Console.Write("*@$+=}");
Console.WriteLine("*@$+=}");
Console.WriteLine("");
Console.WriteLine("1 line empty before");
Console.WriteLine("Press left button to continue");

while (!Fire.ButtonLeft.IsPressed)
{
    Thread.Sleep(10);
}

Console.Clear();

Console.WriteLine("Calibrating the accelerator, do not touch it!");
var acc = Fire.AccelerometerGyroscope;
acc.Calibrate(100);
Console.WriteLine("");
Console.WriteLine("Calibrating the magnetometer, please move it all around");
var mag = Fire.Magnetometer;
mag.CalibrateMagnetometer(100);

Fire.ButtonLeft.Press += (sender, e) =>
{
    Console.ForegroundColor = Color.Yellow;
    Console.CursorLeft = 0;
    Console.CursorTop = 0;
    Console.Write($"Left Pressed  ");
};

Fire.ButtonCenter.Press += (sender, e) =>
{
    Console.ForegroundColor = Color.Yellow;
    Console.CursorLeft = 0;
    Console.CursorTop = 0;
    Console.Write($"Center Pressed");
};

Fire.ButtonRight.Press += (sender, e) =>
{
    Console.ForegroundColor = Color.Yellow;
    Console.CursorLeft = 0;
    Console.CursorTop = 0;
    Console.Write($"Right Pressed ");
};

Console.Clear();

var power = Fire.Power;
Vector3 accVal;
Vector3 gyroVal;
Vector3 magVal;

while (true)
{
    accVal = acc.GetAccelerometer();
    gyroVal = acc.GetGyroscope();
    magVal = mag.ReadMagnetometer();
    var headDir = Math.Atan2(magVal.X, magVal.Y) * 180.0 / Math.PI;
    Console.ForegroundColor = Color.Green;
    Console.CursorLeft = 0;
    Console.CursorTop = 1;
    Console.WriteLine("Accelerator:");
    Console.WriteLine($"  x={accVal.X:N2} ");
    Console.WriteLine($"  y={accVal.Y:N2} ");
    Console.WriteLine($"  z={accVal.Z:N2} ");
    Console.ForegroundColor = Color.AliceBlue;
    Console.WriteLine("Gyroscope:");
    Console.WriteLine($"  x={gyroVal.X:N2}  ");
    Console.WriteLine($"  y={gyroVal.Y:N2}  ");
    Console.WriteLine($"  z={gyroVal.Z:N2}  ");
    Console.ForegroundColor = Color.Coral;
    Console.CursorLeft = Console.WindowWidth / 2 - 2;
    Console.CursorTop = 1;
    Console.Write("Magnetometer:");
    Console.CursorLeft = Console.WindowWidth / 2 - 2;
    Console.CursorTop++;
    Console.Write($"  x={magVal.X:N2}   ");
    Console.CursorLeft = Console.WindowWidth / 2 - 2;
    Console.CursorTop++;
    Console.Write($"  y={magVal.Y:N2}   ");
    Console.CursorLeft = Console.WindowWidth / 2 - 2;
    Console.CursorTop++;
    Console.Write($"  z={magVal.Z:N2}   ");
    Console.CursorLeft = Console.WindowWidth / 2 - 2;
    Console.CursorTop++;
    Console.Write($"  h={headDir:N2}  ");
    Console.ForegroundColor = Color.DarkBlue;
    Console.CursorLeft = Console.WindowWidth / 2 - 2;
    Console.CursorTop = 6;
    Console.Write("Power:");
    Console.CursorLeft = Console.WindowWidth / 2 - 2;
    Console.CursorTop++;
    Console.BackgroundColor = power.IsCharging ? Color.Black : Color.Red;
    Console.Write($"  Charging {power.IsCharging}");
    Console.BackgroundColor = Color.Black;
    Console.Write("  ");
    Console.CursorLeft = Console.WindowWidth / 2 - 2;
    Console.CursorTop++;
    Console.Write($"  Full {power.IsBatteryFull} ");
    Thread.Sleep(20);
}

void ColorWipe(Ws28xx neo, System.Drawing.Color color, int count)
{
    BitmapImage img = neo.Image;
    for (var i = 0; i < count; i++)
    {
        img.SetPixel(i, 0, color);
        neo.Update();
    }
}

void Rainbow(Ws28xx neo, int count, int iterations = 1)
{
    BitmapImage img = neo.Image;
    for (var i = 0; i < 255 * iterations; i++)
    {
        for (var j = 0; j < count; j++)
        {
            img.SetPixel(j, 0, Wheel((i + j) & 255));
        }

        neo.Update();
    }
}

System.Drawing.Color Wheel(int position)
{
    if (position < 85)
    {
        return System.Drawing.Color.FromArgb(position * 3, 255 - position * 3, 0);
    }
    else if (position < 170)
    {
        position -= 85;
        return System.Drawing.Color.FromArgb(255 - position * 3, 0, position * 3);
    }
    else
    {
        position -= 170;
        return System.Drawing.Color.FromArgb(0, position * 3, 255 - position * 3);
    }
}