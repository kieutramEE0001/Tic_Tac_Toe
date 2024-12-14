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
            get => chessboard;
            set => chessboard = value;
        }
        //Tạo danh sách người chơi
        private List<Player> player;
        public List<Player> Player 
        {
            get => player;
            set => player = value;
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
            get => playername;
            set => playername = value;
        }
        //Tạo biến quân cờ
        private PictureBox playermark;
        public PictureBox PlayerMark 
        {
            get => playermark;
            set => playermark = value;
        }

        //Tạo ma trận quân cờ để xác định thắng thua
        private List<List<Button>> chessmatrix;
        public List<List<Button>> Chess_Matrix 
        {
            get => chessmatrix;
            set => chessmatrix = value;
        }
        //Tạo event thông báo người chơi đã click vào 1 ô trong bàn cờ
        private event EventHandler playermarked;
        public event EventHandler PlayerMarked
        {
            add 
            {
                playermarked += value;
            }
            remove
            {
                playermarked -= value;
            }
        }
        //Tạo event thông báo kết thúc game
        private event EventHandler endedgame;
        public event EventHandler EndedGame
        {
            add
            {
                endedgame += value;
            }
            remove
            {
                endedgame -= value;
            }
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
                //Lấy ảnh số 12 từ file Resources để gán cho Player 1
                new Player("Player 1", Image.FromFile(Application.StartupPath + "\\Resources\\12.png")),
                //Lấy ảnh số 13 từ file Resources để gán cho Player 2
                new Player("Player 2", Image.FromFile(Application.StartupPath + "\\Resources\\13.png"))
            };
            CurrentPlayer = 0;
        }
        #endregion
        //Vùng phương thức quản lý bàn cờ
        #region Methods
        //Hàm vẽ bàn cờ
        public void Draw_ChessBoard()
        {
            //Cho phép nhấn vào 1 button bất kỳ
            ChessBoard.Enabled = true;

            //Xóa hết tất cả đối tượng trước khi tạo bàn cờ mới
            ChessBoard.Controls.Clear();

            //Khởi tạo lại lượt chơi, bắt đầu từ player 1
            CurrentPlayer = 0;
            Change_Player_Turn();

            //Bắt đầu vẽ bàn cờ
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
                        Location = new Point(j * Const.Chess_W, i * Const.Chess_W),
                        BackColor = Color.White,
                        //Dàn hình ảnh trải đều trên button
                        BackgroundImageLayout = ImageLayout.Stretch,
                        //Lưu index hàng của button
                        Tag = i.ToString(),
                    };

                    button.Click += button_Click;

                    ChessBoard.Controls.Add(button);
                    Chess_Matrix[i].Add(button);
                }
            }
        }
        void button_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;

            //Kiểm tra xem ô đã được đánh hay chưa
            if (button.BackgroundImage != null)
            {
                return;
            }

            Mark(button);

            //Tạo biến đếm các ô đã được đánh trong bàn cờ
            int count = 0;

            foreach (var i in Chess_Matrix)
            {
                foreach (var j in i)
                {
                    if (j.BackgroundImage != null)
                        count++;
                }    
            }

            Change_Player_Turn();

            //Gọi event playermarked 
            if (playermarked != null)
                playermarked(this, new EventArgs());

            //Kiểm tra trường hợp hòa 
            if (count == Const.ChessBoard_W * Const.ChessBoard_H)
                EndwDraw();

            //Kiểm tra trường hợp thắng
            if (IsWinning(button))
            {
                EndGame();
            }

        }
        //Hàm thông báo kết thúc Game nếu có người thắng
        private void EndGame()
        {
            if (endedgame != null) 
                endedgame(this, new EventArgs());
            CurrentPlayer = CurrentPlayer == 1 ? 0 : 1;
            MessageBox.Show($"{Player[currentplayer].Name} đã chiến thắng! Kết thúc Game!");
        }
        //Hàm thông báo kết thúc nếu hòa
        private void EndwDraw()
        {
            if (endedgame != null)
                endedgame(this, new EventArgs());
            MessageBox.Show("Hòa nhau! Kết thúc Game!");
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
            //Trả về kết quả duyệt, 5 quân cờ liên tiếp trên hàng dọc là thắng
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
            //Trả về kết quả duyệt, 5 quân cờ liên tiếp trên đường chéo chính là thắng
            return countTopLeft + countBottomRight == 5;
        }
        //Hàm kiểm tra theo đường chéo phụ
        private bool isSecondDiagonal(Button button)
        {
            Point point = GetChessPoint(button);
            //Duyệt lên phía trên, bên phải
            int countTopRight = 0;
            for (int i = 0; i <= point.Y; i++)
            {
                if (point.Y - i < 0 || point.X + i >= Const.ChessBoard_W)
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
            for (int i = 1; i <= Const.ChessBoard_H - point.Y; i++)
            {
                if (point.Y + i >= Const.ChessBoard_H || point.X - i < 0)
                    break;
                if (Chess_Matrix[point.Y + i][point.X - i].BackgroundImage == button.BackgroundImage)
                {
                    countBottomLeft++;
                }
                else
                    break;
            }
            //Trả về kết quả duyệt, 5 quân cờ liên tiếp trên đường chéo phụ là thắng
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
