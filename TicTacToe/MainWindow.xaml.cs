using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TicTacToe
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Private Members
        ///  <summary>
        /// Holds the current result of cells in the active game 
        private MarkType[] mResults;
        private bool mPlayer1Turn;
        private bool mGameEnded;
        /// </summary>
        #endregion
        #region Constructor
        /// <summary>
        /// Default conructor
        /// </summary>

        public MainWindow()
        {
            InitializeComponent();
            newgame();
        }
        #endregion
        #region NewGameSetup Clear board 
        private void newgame()
        {
            mResults = new MarkType[9];
            StatusText.Text = "";
            //explicitly set each item. 
            for (int i = 0; i < mResults.Length; i++)
            {
                mResults[i] = MarkType.Free;
            }
            //make sure P1 starts the game. 
            mPlayer1Turn = true;
            foreach (UIElement element in gridContainer.Children)
            {
                if (element.GetType() == typeof(Button))
                {
                    Button button = (Button)element;
                    button.Content = string.Empty;
                    button.Background = Brushes.White;
                    button.Foreground = Brushes.Blue;
                }
            }


        }
        #endregion
        private Brush ButtonColor(bool mPlayer1Turn)
        {
            if (mPlayer1Turn) { return Brushes.Black; } else { return Brushes.Blue; }
        }

        private string PlayerPiece(bool mPlayer1Turn)
        {
            if (mPlayer1Turn) { return "X"; } else { return "O"; }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Setup and create the game 
            SetupGame(mGameEnded);
            //click the nought and cross on the button clicked  
            PlacePiece(sender);
            //check for winner or no spaces left
            mGameEnded = CheckForWinner();
            //if not game ended change player 
            if (!mGameEnded) ChangePlayer();



        }

        private void PlacePiece(object sender)
        {
            //cast sender to button 
            var thisButton = (Button)sender;
            //find buttons in array
            var column = Grid.GetColumn(thisButton);
            var row = Grid.GetRow(thisButton);

            var index = column + (row * 3);

            //don't do anything if cell has value
            if (mResults[index] != MarkType.Free)
                return;

            mResults[index] = mPlayer1Turn ? MarkType.Cross : MarkType.Nought;
            thisButton.Content = PlayerPiece(mPlayer1Turn);
            thisButton.Foreground = ButtonColor(mPlayer1Turn);

        }

        private void SetupGame(bool mGameEnded)
        {
            if (mGameEnded)
            {
                newgame();
                return;
            }
        }

        private bool CheckForWinner()
        {
            var bGameEnd = false;

            #region Horizontal Checks 
            for (int i = 0; i <= 6; i += 3)
            {
                //row 1
                if (mResults[i] != MarkType.Free && ((mResults[i]) & mResults[i + 1] & mResults[i + 2]) == mResults[i])
                {
                    if (i == 0)
                    {
                        Button1.Background = Button2.Background = Button3.Background = Brushes.Green;
                    }

                    if (i == 3)
                    {
                        Button4.Background = Button5.Background = Button6.Background = Brushes.Green;
                    }

                    if (i == 6)
                    {
                        Button7.Background = Button8.Background = Button9.Background = Brushes.Green;
                    }


                    bGameEnd = true;
                }
            }
            #endregion

            #region Vertical Checks 
            for (int i = 0; i <= 2; i++)
            {
                if (mResults[i] != MarkType.Free && ((mResults[i]) & mResults[i + 3] & mResults[i + 6]) == mResults[i])
                {
                    if (i == 0)
                    {
                        Button1.Background = Button4.Background = Button7.Background = Brushes.Green;
                    }

                    if (i == 1)
                    {
                        Button2.Background = Button5.Background = Button8.Background = Brushes.Green;
                    }

                    if (i == 2)
                    {
                        Button3.Background = Button6.Background = Button9.Background = Brushes.Green;
                    }
                    bGameEnd = true;
                }

            }
            #endregion

            #region Diagonal Checks 
            for (int i = 0; i <= 1; i++)
            {
                if (mResults[(0 + (i * 2))] != MarkType.Free && ((mResults[0 + (i * 2)]) & mResults[4] & mResults[8 - (2 * i)]) == mResults[0 + (i * 2)])
                {
                    if (i == 0) { Button1.Background = Button5.Background = Button9.Background = Brushes.Green; }
                    if (i == 1) { Button3.Background = Button5.Background = Button7.Background = Brushes.Green; }

                    bGameEnd = true;
                }

            }
            #endregion

            if (bGameEnd) { StatusText.Text = "Winner- Click a square to play again"; }
            return bGameEnd;

        }

        private void ChangePlayer()
        {
            mPlayer1Turn = !mPlayer1Turn;
        }

        private bool CheckNoSpacesLeft(bool mGameEnded)
        {
            mGameEnded = true; //easier to assume game is ended then if find a free spare state is is not ended. 
            foreach (var item in mResults)
            {
                if (item == MarkType.Free)
                {
                    mGameEnded = false;
                    break;
                }
            }

            if (mGameEnded == true)
            {
                gridContainer.Children.Cast<Button>().ToList().ForEach(button =>
                {
                    mGameEnded = false;

                    //change background foreground and content to default value                 
                    button.Background = Brushes.Orange;
                });
            }
            return mGameEnded;
        }

    }
}
