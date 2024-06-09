﻿using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using WindowsInput;

namespace BSS_Starsaw_macro
{
    public partial class Form1 : Form
    {
        InputSimulator simulator = new InputSimulator();
        private Thread _macroThread;
        private bool _isMacroRunning;

        public Form1()
        {
            InitializeComponent();
            KeyboardHook.SetHook();
            KeyboardHook.F6Pressed += KeyboardHook_F6Pressed;
            KeyboardHook.F5Pressed += KeyboardHook_F5Pressed;
            this.FormClosing += Form1_FormClosing;
        }

        private void btn_github_click(object sender, EventArgs e)
        {
            OpenUrlInBrowser("https://github.com/");
        }

        private void OpenUrlInBrowser(string url)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void btn_stop_click(object sender, EventArgs e)
        {
            StopMacro();
            this.Close();
        }

        private void btn_start_click(object sender, EventArgs e)
        {
            StartMacro();
        }

        private void StartMacro()
        {
            if (_isMacroRunning) return;

            _isMacroRunning = true;
            _macroThread = new Thread(() =>
            {
                Thread.Sleep(3000);
                while (_isMacroRunning)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        if (!_isMacroRunning) return;
                        simulator.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_6);
                        Thread.Sleep(1100);
                    }

                    int totalSleep = 41700;
                    int sleepInterval = 100; // 100 ms intervals
                    int iterations = totalSleep / sleepInterval;
                    for (int j = 0; j < iterations; j++)
                    {
                        if (!_isMacroRunning) return;
                        Thread.Sleep(sleepInterval);
                    }
                }
            });
            _macroThread.Start();
        }

        private void StopMacro()
        {
            _isMacroRunning = false;
            if (_macroThread != null && _macroThread.IsAlive)
            {
                _macroThread.Join();
            }
        }

        private void KeyboardHook_F6Pressed(object sender, EventArgs e)
        {
            StopMacro();
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(this.Close));
            }
            else
            {
                this.Close();
            }
        }

        private void KeyboardHook_F5Pressed(object sender, EventArgs e)
        {
            StartMacro();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopMacro();
            KeyboardHook.Unhook();
        }
    }
}
