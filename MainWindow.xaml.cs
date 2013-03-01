using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.Windows.Threading;
using System.ComponentModel;

namespace WpfExp
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private Button buttonClicking;
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            buttonClicking = sender as Button;
            if (null != buttonClicking)
            {
                buttonClicking.IsEnabled = false;
                switch (buttonClicking.Name)
                {
                    case "button1":
                        RefreshByThread("RefreshByThread");
                        break;
                    case "button2":
                        RefreshByDelegateInvoke("RefreshByDelegateInvoke");
                        break;
                    case "button3":
                        RefreshByDispatcher("RefreshByDispatcher");
                        break;
                    case "button4":
                        RefreshByDispatcherTimer("RefreshByDispatcherTimer");
                        break;
                    case "button5":
                        RefreshByBackgroundWorker("RefreshByBackgroundWorker");
                        break;
                    default:
                        break;
                }
            }
        }
        //耗时操作
        private bool HardWork()
        {
            Thread.Sleep(5000);//模拟耗时操作
            return true;
        }
        //更新UI
        private void UpdateUI(string text)
        {
            //当前线程与UI线程不属于同一线程
            this.Dispatcher.Invoke(new Action<string>((obj) =>
            {
                this.textBlock1.Text = text;
                this.buttonClicking.IsEnabled = true;
            }), text);
        }
        //using thread
        private void RefreshByThread(string text)
        {
            Thread thread = new Thread(new ParameterizedThreadStart((param) => {
                bool result = HardWork();
                Console.WriteLine("******************{0}*******************", result);
                UpdateUI(text);
            }));
            thread.IsBackground = true;
            thread.Start(text);
        }
        //using delegate
        private void RefreshByDelegateInvoke(string text)
        {
            //Predicate<void> act = new Predicate<void>(this.HardWork);
            Func<bool> act = new Func<bool>(this.HardWork);
            //1.使用回调方法 得到结果
            act.BeginInvoke(new AsyncCallback((asyncResult) =>
            {
                if (null != asyncResult)
                {
                    bool result = (bool)(asyncResult.AsyncState as Func<bool>).EndInvoke(asyncResult);
                    Console.WriteLine("******************{0}*******************", result);
                    UpdateUI(text);
                }
            }), act);
            //2.使用轮询 得到结果
            //IAsyncResult asyncResult = act.BeginInvoke(null, act);
            //while (!asyncResult.IsCompleted)
            //{
            //    Thread.Sleep(10);
            //    //做其它事情
            //}
            //bool result = (asyncResult.AsyncState as Func<bool>).EndInvoke(asyncResult);
            //Console.WriteLine("******************{0}*******************", result);
            //UpdateUI(text);
            //3.一直等待结果，阻塞
            //IAsyncResult asyncResult = act.BeginInvoke(null, act);
            //bool result = (asyncResult.AsyncState as Func<bool>).EndInvoke(asyncResult);
            //Console.WriteLine("******************{0}*******************", result);
            //UpdateUI(text);
        }
        //using dispatcher
        private void RefreshByDispatcher(string text)
        {
            new Thread(new ParameterizedThreadStart((param) =>
            {
                bool result = HardWork();
                Console.WriteLine("******************{0}*******************", result);
                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(()=>{
                    this.textBlock1.Text = param.ToString();
                    this.button1.IsEnabled = true;
                }));
            })).Start(text);
        }
        //using dispatcherTimer
        public static readonly object lockobj = new object();
        
        private void RefreshByDispatcherTimer(string text)
        {
            int dispatcherCounter = 0;
            DispatcherTimer _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(1);
            _timer.Tick += new EventHandler(new Action<object, object>((sender, e) =>
            {
                lock (lockobj)
                {
                    if (dispatcherCounter == 1)
                        return;
                    dispatcherCounter = 1;
                }
                bool result = HardWork();
                Console.WriteLine("******************{0}*******************", result);
                UpdateUI(text);
                _timer.Stop();
            }));
            //_timer.Interval = TimeSpan.FromMilliseconds(10000);
            //_timer.Tick += new EventHandler(new Action<object, object>((sender, e) =>
            //{
            //    bool result = HardWork();
            //    Console.WriteLine("******************{0}*******************", result);
            //    UpdateUI(text);
            //    _timer.Stop();
            //}));
            _timer.Start();
        }
        //using backgroundworker
        private void RefreshByBackgroundWorker(string text)
        {
            BackgroundWorker _backgroundWorker = new BackgroundWorker();
            _backgroundWorker.WorkerReportsProgress = true;
            _backgroundWorker.WorkerSupportsCancellation = true;
            bool result = false;
            _backgroundWorker.DoWork += (sender, e) =>
            {
                result = HardWork();
            };
            _backgroundWorker.RunWorkerCompleted += (sender, e) =>
            {
                Console.WriteLine("******************{0}*******************", result);
                UpdateUI(text);
            };
            _backgroundWorker.RunWorkerAsync();
        }
    }
}
