﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tic_Tac_Toe
{
    public partial class Tic_Tac_Toe : Form
    {
        #region Properties
        ChessBoardManage ChessBoard;
        #endregion
        public Tic_Tac_Toe()
        {
            InitializeComponent();
            ChessBoard = new ChessBoardManage(pnlChessBoard);
            ChessBoard.Draw_ChessBoard();
        }

    }

  
    
}