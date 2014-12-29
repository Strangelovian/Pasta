namespace KeyBoardStux
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Collections.Concurrent;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Input;


    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly IEnumerable<Key> typableKeys;

        private IDictionary<Key, long> pressedKeyRegister;

        static MainWindow()
        {
            var keys = ((Key[])Enum.GetValues(typeof(Key)));
            MainWindow.typableKeys = keys.Where(key => key != Key.None);
        }

        public MainWindow()
        {
            InitializeComponent();

            pressedKeyRegister = new Dictionary<Key, long>();
            this.PreviewKeyDown += MainWindow_KeyDown;
            this.PreviewKeyUp += MainWindow_KeyUp;
        }

        private void MainWindow_KeyUp(object sender, KeyEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                pressedKeyRegister.Remove(e.Key);
                this.Console.AppendText(String.Format("{0} released\n", e.Key));
                this.Console.ScrollToEnd();
            });
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                if (!pressedKeyRegister.Keys.Contains(e.Key))
                {
                    pressedKeyRegister.Add(e.Key, DateTime.Now.Ticks);
                    this.Console.AppendText(String.Format("{0} pressed\n", e.Key));
                    this.Console.ScrollToEnd();
                }
            });
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                if (pressedKeyRegister.Keys.Any())
                {
                    long nowTicks = DateTime.Now.Ticks;
                    foreach (KeyValuePair<Key, long> pair in pressedKeyRegister)
                    {
                        long delay = nowTicks - pair.Value;
                        // 0 => 250ms, 1 => 500ms, 2 =>750ms, 3 => 1s
                        // 1ms = 10000 ticks
                        if (delay > (SystemParameters.KeyboardDelay + 1) * 250 * 10000)  
                        {
                            this.Console.AppendText(String.Format("{0} pressed since {1:0.##}\n", pair.Key, ((decimal)delay)/10000000));
                            this.Console.ScrollToEnd();
                        }
                    }
                }
                else
                {
                    this.Console.AppendText("No keys pressed\n");
                    this.Console.ScrollToEnd();
                }
            });
        }

        private static IEnumerable<Key> KeysDown()
        {
            foreach (Key key in typableKeys)
            {
                if (Keyboard.IsKeyDown(key))
                    yield return key;
            }
        }
    }
}
