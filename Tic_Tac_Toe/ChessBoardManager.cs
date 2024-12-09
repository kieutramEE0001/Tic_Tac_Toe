using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tic_Tac_Toe
{
    internal class ChessBoardManager
    {
        //Vùng thuộc tính của Panel cần sử dụng
        #region Properties
        private Panel chessboard;
        public Panel ChessBoard
        {
            get { return chessboard; }
            set { chessboard = value; }
        }
        //Tạo danh sách người chơi
        private List<Player> player;
        public List<Player> Player 
        {
            get { return player; }
            set { player = value; }
        }

        //Tạo biến lưu lượt đánh cờ
        private int currentplayer;
        public int CurrentPlayer 
        { 
            get => currentplayer; 
            set => currentplayer = value; 
        }

        private TextBox playername;
        public TextBox PlayerName 
        {
            get { return playername; }
            set { playername = value; }
        }

        private PictureBox playermark;
        public PictureBox PlayerMark 
        {
            get { return playermark; }
            set { playermark = value; }
        }

        #endregion
        //Vùng phương thức khởi tạo
        #region Initialize
        public ChessBoardManager(Panel chessboard, TextBox playername, PictureBox mark)
        {
            this.ChessBoard = chessboard;
            this.PlayerName = playername;
            this.PlayerMark = mark;
            this.Player = new List<Player>()
            {
                new Player("Player 1", Image.FromFile(Application.StartupPath + "\\Resources\\12.png")),
                new Player("Player 2", Image.FromFile(Application.StartupPath + "\\Resources\\13.png"))
            };
            CurrentPlayer = 0;
        }
        #endregion
        //Vùng phương thức vẽ bàn cờ
        #region Methods
        public void Draw_ChessBoard()
        {
            Button oldbutton = new Button() { Width = 0, Location = new Point(0, 0) };
            for (int i = 0; i < Const.ChessBoard_H; i++)
            {
                for (int j = 0; j < Const.ChessBoard_W; j++)
                {
                    Button button = new Button()
                    {
                        Width = Const.Chess_W,
                        Height = Const.Chess_H,
                        Location = new Point(oldbutton.Location.X + oldbutton.Width, oldbutton.Location.Y),
                        BackColor = Color.White,
                        //Dàn hình ảnh trải đều trên button
                        BackgroundImageLayout = ImageLayout.Stretch,
                    };
                    button.Click += button_Click;
                    ChessBoard.Controls.Add(button);
                    oldbutton = button;
                }
                oldbutton.Location = new Point(0, oldbutton.Location.Y + Const.Chess_H);
                oldbutton.Width = 0;
                oldbutton.Height = 0;

            }
        }

        void button_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            //Kiểm tra xem ô đã được đánh hay chưa
            if (button.BackgroundImage != null)
                return;
            Mark(button);
            Change_Player_Turn();
        }
        private void Mark(Button button)
        {
            button.BackgroundImage = Player[currentplayer].Mark;
            CurrentPlayer = CurrentPlayer == 1 ? 0 : 1;
        }
        //Hàm hoán đổi lượt chơi
        private void Change_Player_Turn()
        {
            PlayerName.Text = Player[currentplayer].Name;
            PlayerMark.Image = Player[currentplayer].Mark;
        }
        #endregion

    }
}
