using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
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
        //Tạo biến tên người chơi
        private TextBox playername;
        public TextBox PlayerName 
        {
            get { return playername; }
            set { playername = value; }
        }
        //Tạo biến quân cờ
        private PictureBox playermark;
        public PictureBox PlayerMark 
        {
            get { return playermark; }
            set { playermark = value; }
        }
        //Tạo ma trận quân cờ để xác định thắng thua
        private List<List<Button>> chessmatrix;
        public List<List<Button>> Chess_Matrix 
        {
            get { return chessmatrix; }
            set { chessmatrix = value; }
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
            Chess_Matrix = new List<List<Button>>();
            for (int i = 0; i < Const.ChessBoard_H; i++)
            {
                Chess_Matrix.Add(new List<Button>());
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
                        //Lưu index hàng của button
                        Tag = i.ToString(),
                    };

                    button.Click += button_Click;

                    ChessBoard.Controls.Add(button);
                    Chess_Matrix[i].Add(button);

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
            if (IsWinning(button))
            {
                EndGame();
            }    
        }
        private void EndGame()
        {
            CurrentPlayer = CurrentPlayer == 1 ? 0 : 1;
            MessageBox.Show($"{Player[currentplayer].Name} đã chiến thắng!");
        }
        private bool IsWinning(Button button)
        {
            return isHorizontal(button) || isVertical(button) || isPrimaryDiagonal(button) || isSecondDiagonal(button);
        }
        //Hàm lấy tọa độ ngang dọc của một điểm trên bàn cờ
        private Point GetChessPoint(Button button)
        {
            int vertical = Convert.ToInt32(button.Tag);
            int horizontal = Chess_Matrix[vertical].IndexOf(button);
            Point point = new Point(horizontal, vertical);
            return point;
        }
        //Hàm kiểm tra theo hàng ngang
        private bool isHorizontal(Button button)
        {
            Point point = GetChessPoint(button);
            //Duyệt về bên trái, kiểm tra số ô trùng khớp với quân cờ 
            int countLeft = 0;
            for (int i = point.X; i>=0; i--)
            {
                if (Chess_Matrix[point.Y][i].BackgroundImage == button.BackgroundImage)
                {
                    countLeft++;
                }
                else
                    break;
            }  
            //Duyệt về bên phải
            int countRight = 0;
            for (int i = point.X+1; i<Const.ChessBoard_W;i++)
            {
                if (Chess_Matrix[point.Y][i].BackgroundImage == button.BackgroundImage)
                {
                    countRight++;
                }    
                else
                    break;
            }    
            //Trả về kết quả duyệt, 5 quân cờ liên tiếp trên hàng ngang là thắng
            return countLeft + countRight == 5;
        }
        //Hàm kiểm tra theo hàng dọc
        private bool isVertical(Button button)
        {
            Point point = GetChessPoint(button);
            //Duyệt lên trên, kiểm tra số quân cờ trùng khớp
            int countTop = 0;
            for (int i = point.Y;i>=0;i--)
            {
                if (Chess_Matrix[i][point.X].BackgroundImage == button.BackgroundImage)
                {
                    countTop++;
                }    
                else 
                    break;
            }
            //Duyệt xuống dưới
            int countBottom = 0;
            for (int i = point.Y+1; i<Const.ChessBoard_H; i++)
            {
                if (Chess_Matrix[i][point.X].BackgroundImage == button.BackgroundImage)
                {
                    countBottom++;
                }
                else
                    break;
            }    
            return countTop + countBottom == 5;
        }
        //Hàm kiểm tra theo đường chéo chính
        private bool isPrimaryDiagonal(Button button)
        {
            Point point = GetChessPoint(button);
            //Duyệt lên phía trên, bên trái
            int countTopLeft = 0;
            for (int i = 0; i <= point.X; i++)
            {
                if (point.Y - i < 0 || point.X - i < 0)
                    break;
                if (Chess_Matrix[point.Y - i][point.X - i].BackgroundImage == button.BackgroundImage)
                {
                    countTopLeft++;
                }
                else
                    break;
            }
            //Duyệt xuống phía dưới, bên phải
            int countBottomRight = 0;
            for (int i = 1; i <= Const.ChessBoard_W - point.X; i++)
            {
                if (point.Y + i >= Const.ChessBoard_H || point.X + i >= Const.ChessBoard_W)
                    break;
                if (Chess_Matrix[point.Y + i][point.X + i].BackgroundImage == button.BackgroundImage)
                {
                    countBottomRight++;
                }
                else
                    break;
            }
            return countTopLeft + countBottomRight == 5;
        }
        //Hàm kiểm tra theo đường chéo phụ
        private bool isSecondDiagonal(Button button)
        {
            Point point = GetChessPoint(button);
            //Duyệt lên phía trên, bên phải
            int countTopRight = 0;
            for (int i = 0; i <= point.X; i++)
            {
                if (point.Y - i < 0 || point.X + i > Const.ChessBoard_W)
                    break;
                if (Chess_Matrix[point.Y - i][point.X + i].BackgroundImage == button.BackgroundImage)
                {
                    countTopRight++;
                }
                else
                    break;
            }
            //Duyệt xuống phía dưới, bên trái
            int countBottomLeft = 0;
            for (int i = 1; i <= Const.ChessBoard_W - point.X; i++)
            {
                if (point.Y + i >= Const.ChessBoard_H || point.X - i > 0)
                    break;
                if (Chess_Matrix[point.Y + i][point.X - i].BackgroundImage == button.BackgroundImage)
                {
                    countBottomLeft++;
                }
                else
                    break;
            }
            return countTopRight + countBottomLeft == 5;
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
