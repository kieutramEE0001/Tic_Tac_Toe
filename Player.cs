using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tic_Tac_Toe
{
    public class Player
    {
        private string name;
        public string Name 
        {
            get => name; 
            set => name = value; 
        }

        private Image mark;
        public Image Mark 
        { 
            get => mark; 
            set => mark = value; 
        }

        public Player(string name, Image mark)
        {
            //Biểu thị cho lớp hiện tại, ứng dụng thứ tự ưu tiên biến cục bộ và biến toàn cục
            this.Name = name;
            this.Mark = mark;
        }
       


    }
}
