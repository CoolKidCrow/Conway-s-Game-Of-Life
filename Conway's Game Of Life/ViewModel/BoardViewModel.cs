using Conway_s_Game_Of_Life.Core;
using Conway_s_Game_Of_Life.Model;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace Conway_s_Game_Of_Life.ViewModel
{
    class BoardViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<ButtonModel> _board = new ObservableCollection<ButtonModel>();
        private int _columnSize = 50;
        private int _rowSize = 50;
        private string _playPauseUrl;
        private DispatcherTimer _timer;

        public ObservableCollection<ButtonModel> Board
        {
            get => _board;
            set
            {
                _board = value;
                OnPropertyChanged();
            }
        }

        public int ColumnSize
        {
            get => _columnSize;
            set
            {
                _columnSize = value;
                OnPropertyChanged();
            }
        }

        public int RowSize
        {
            get => _rowSize;
            set
            {
                _rowSize = value;
                OnPropertyChanged();
            }
        }

        public string PlayPauseUrl
        {
            get => _playPauseUrl;
            set
            {
                _playPauseUrl = value;
                OnPropertyChanged();
            }
        }

        public ICommand CommandStepBoard { get; }
        public ICommand CommandSetSquare { get; }
        public ICommand CommandClearBoard { get; }


        public BoardViewModel()
        {
            InitializeGrid(null);
            PlayPauseUrl = "/Images/play-button.png";

            CommandSetSquare = new RelayCommand<int>(SetSquare);
            CommandStepBoard = new RelayCommand<object?>(PlayPause);
            CommandClearBoard = new RelayCommand<object?>(InitializeGrid);

            _timer = new DispatcherTimer(DispatcherPriority.Render);
            _timer.Tick += On_Tick;
            _timer.Interval = TimeSpan.FromMilliseconds(300);
        }

        private void SetSquare(int index) 
        {
            Board[index].Value = (Board[index].Value == 0) ? 1 : 0;
        }

        private void On_Tick(object sender, EventArgs e) => StepBoard(null);


        private void InitializeGrid(object? obj)
        {
            Board.Clear();

            for (int i = 0; i < RowSize; i++)
            {
                for (int j = 0; j < ColumnSize; j++)
                    Board.Add(new ButtonModel(0, i * RowSize + j));
            }
        }


        public void PlayPause(object? obj)
        {
            PlayPauseUrl = (PlayPauseUrl == "/Images/play-button.png") ? "/Images/pause-button.png" : "/Images/play-button.png";
            if(PlayPauseUrl == "/Images/pause-button.png")
                _timer.Start();
            else
                _timer.Stop();
        }

        public void StepBoard(object? obj)
        {
            int[] oldBoard = new int[Board.Count];
            for (int i = 0; i < Board.Count; i++)
                oldBoard[i] = Board[i].Value;


            for (int i = 0; i < RowSize; i++)
            {
                for (int j = 0; j < ColumnSize; j++)
                    Board[Convert2DIndexTo1D(i, j)].Value = (oldBoard[Convert2DIndexTo1D(i, j)] == 0) ? CheckIsStillDead(oldBoard, i, j) : CheckIsStillAlive(oldBoard, i, j);
            }
        }

        private int CheckIsStillDead(int[] board, int i, int j)
        {
            int neighborCount = GetNeighborCount(board, i, j);
            return (neighborCount == 3) ? 1 : 0;
        }

        private int CheckIsStillAlive(int[] board, int i, int j)
        {
            int neighborCount = GetNeighborCount(board, i, j);
            return (neighborCount == 2 || neighborCount == 3) ? 1 : 0;
        }

        private int GetNeighborCount(int[] board, int i, int j)
        {
            int count = 0;
            count += (i + 1 < RowSize) ? board[Convert2DIndexTo1D(i + 1, j)] : 0; //down
            count += (j + 1 < ColumnSize) ? board[Convert2DIndexTo1D(i, j + 1)] : 0; //right
            count += (i + 1 < RowSize && j + 1 < ColumnSize) ? board[Convert2DIndexTo1D(i + 1, j + 1)] : 0; //down right
            count += (i + 1 < RowSize && j - 1 >= 0) ? board[Convert2DIndexTo1D(i + 1, j - 1)] : 0; //down left
            count += (i - 1 >= 0) ? board[Convert2DIndexTo1D(i - 1, j)] : 0; //up
            count += (i - 1 >= 0 && j + 1 < ColumnSize) ? board[Convert2DIndexTo1D(i - 1, j + 1)] : 0; //up right
            count += (i - 1 >= 0 && j - 1 >= 0) ? board[Convert2DIndexTo1D(i - 1, j - 1)] : 0; //up left
            count += (j - 1 >= 0) ? board[Convert2DIndexTo1D(i, j - 1)] : 0; //left


            return count;
        }

        private int Convert2DIndexTo1D(int row, int column)
        {
            return row * RowSize + column;
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }
}
