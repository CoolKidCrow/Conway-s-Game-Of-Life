using Conway_s_Game_Of_Life.Core;
using System;
using System.Diagnostics;
using System.Numerics;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Conway_s_Game_Of_Life
{
    class ViewModel : ObservableObject
    {
        private Model model;
        private DispatcherTimer timer;
        private object _gridView;

        public object gridView
        {
            get { return _gridView; }
            set 
            { 
                _gridView = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand PlayPauseCommand { get; private set; }
        public RelayCommand ClearCommand { get; private set; }

        private UniformGrid _gridSource;

        private Button[,] _buttons;


        private int _gridSize = 100;
        private bool _isPlaying = false;

        public int gridSize
        {
            get { return _gridSize; }
            set
            {
                if (_gridSize != value)
                {
                    _gridSize = value;

                    CreateNewGrid();
                }
            }
        }

        private static SolidColorBrush COLOR_BLACK = new SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 0, 0));
        private static SolidColorBrush COLOR_WHITE = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 255, 255));


        public ViewModel()
        {
            PlayPauseCommand = new RelayCommand(o => {
                PlayPause();
            });
            ClearCommand = new RelayCommand(o => {
                Clear();
            });

            model = new Model(_gridSize, _gridSize);
            _gridSource = new UniformGrid();
            _gridSource.Margin = new System.Windows.Thickness(25);
            _gridView = _gridSource;

            CreateNewGrid();

            timer = new DispatcherTimer();
            timer.Tick += On_Tick;
            timer.Interval = TimeSpan.FromMilliseconds(400);
        }

        private void On_Tick(object sender, EventArgs e) 
        {
            model.GetStep();
            UpdateButtonBoard();
        }


        private void CreateNewGrid()
        {
            _gridSource.Children.Clear();
            _gridSource.Columns = _gridSize;
            _gridSource.Rows = _gridSize;

            _buttons = new Button[_gridSize, _gridSize];
            model.InitNewBoard(_gridSize, _gridSize);
            for (int i = 0; i < _buttons.GetLength(0); i++)
            {
                for(int j = 0; j < _buttons.GetLength(1); j++)
                {
                    _buttons[i, j] = new Button();
                    _buttons[i, j].Background = COLOR_WHITE;
                    _buttons[i, j].BorderThickness = new System.Windows.Thickness(.1);
                    _buttons[i, j].Tag = new  Vector2(i, j);
                    _buttons[i, j].Click += (sender, e) => {
                        Button button = (Button)sender;
                        Vector2 tag = (Vector2)button.Tag;

                        int set = (model.GetBoardItem((int)tag.X, (int)tag.Y) == 1) ? 0 : 1;
                        model.SetBoard((int)tag.X, (int)tag.Y, set);

                        button.Background = (model.GetBoardItem((int)tag.X, (int)tag.Y) == 1) ? COLOR_BLACK : COLOR_WHITE;
                    };
                    _gridSource.Children.Add(_buttons[i, j]);
                }
            }

            OnPropertyChanged();
        }


        public void PlayPause()
        {
            _isPlaying = !_isPlaying;

            if (_isPlaying ) 
            {
                timer.Start();
            }else
            {
                timer.Stop();
            }
        }


        public void Clear() 
        {
            //clear
            model.ClearBoard();
            UpdateButtonBoard();
        }

        private void UpdateButtonBoard()
        {
            int[,] board = model.GetBoard();

            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    _buttons[i, j].Background = (board[i, j] == 1) ? COLOR_BLACK : COLOR_WHITE;
                }
            }
        }


    }
}
