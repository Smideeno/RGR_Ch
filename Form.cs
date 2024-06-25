using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PuzzleGmae
{
    public partial class Form1 : Form
    {
        private PictureBox[,] puzzlePieces;
        private int rows = 4; // Number of rows of puzzle pieces defult
        private int columns = 4; // Number of columns of puzzle pieces defult
        private int pieceWidth;
        private int pieceHeight;

        private PictureBox selectedPiece;
        private Point originalPosition;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void CreateAndDisplayPuzzlePieces()
        {
            puzzlePieces = pictureBoxPuzzlePieces[rows, columns];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    puzzlePieces[i, j].SizeMode = PictureBoxSizeMode.StretchImage;
                    puzzlePieces[i, j].Image = new Bitmap(pieceWidth, pieceHeight);

                    Graphics g = Graphics.FromImage(puzzlePieces[i, j].Image);
                    g.DrawImage(pictureBoxStart.Image, new Rectangle(0, 0, pieceWidth, pieceHeight), new Rectangle(j * pieceWidth, i * pieceHeight, pieceWidth, pieceHeight), GraphicsUnit.Pixel);
                    g.Dispose();

                    puzzlePieces[i, j].MouseDown += PuzzlePiece_MouseDown;
                    puzzlePieces[i, j].MouseMove += PuzzlePiece_MouseMove;
                    puzzlePieces[i, j].MouseUp += PuzzlePiece_MouseUp;
                }
            }

            DisplayPuzzlePieces();


        }
        private void ShufflePuzzlePieces()
        {
            List<PictureBox> allPieces = puzzlePieces.Cast<PictureBox>().ToList();
            Random rnd = new Random();

            for (int i = 0; i < allPieces.Count; i++)
            {
                int randomIndex = rnd.Next(i, allPieces.Count);
                PictureBox temp = allPieces[randomIndex];
                allPieces[randomIndex] = allPieces[i];
                allPieces[i] = temp;
            }

            puzzlePieces = new PictureBox[rows, columns];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    puzzlePieces[i, j] = allPieces[i * columns + j];
                }
            }
        }

        private void DisplayPuzzlePieces()
        {
            int offsetX = pictureBoxStart.Location.X;
            int offsetY = pictureBoxStart.Location.Y + 100;

            this.Controls.Remove(pictureBoxStart);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    puzzlePieces[i, j].Location = new Point(offsetX + j * pieceWidth, offsetY + i * pieceHeight);
                    this.Controls.Add(puzzlePieces[i, j]);
                }
            }
        }


        private void buttonShowPicture_Click(object sender, EventArgs e)
        {


            OpenFileDialog opf = new OpenFileDialog();
            opf.Title = "Select Image";
            opf.Filter = "Image File *.jpg; *.png; *.bmp; | *.jpg; *.png; *.bmp;";
            if (opf.ShowDialog() == DialogResult.OK)
            {
                Bitmap image = new Bitmap(opf.FileName);
                //pictureBoxStart.Image = image;

                int desiredWidth = 600;
                int desiredHeight = 400; 
                pictureBoxStart.Image = ResizeImage(image, desiredWidth, desiredHeight);
            }

            pieceWidth = pictureBoxStart.Image.Width / columns;
            pieceHeight = pictureBoxStart.Image.Height / rows;

            CreateAndDisplayPuzzlePieces();
            ShufflePuzzlePieces();

            DisplayPuzzlePieces();

        }
        private Image ResizeImage(Image image, int width, int height)
        {
            Bitmap result = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(result))
            {
                g.DrawImage(image, 0, 0, width, height);
            }
            return result;
        }

        private void PuzzlePiece_MouseDown(object sender, MouseEventArgs e)
        {
            selectedPiece = (PictureBox)sender;
            originalPosition = e.Location;
            selectedPiece.BringToFront(); 
        }

        private void PuzzlePiece_MouseMove(object sender, MouseEventArgs e)
        {
            if (selectedPiece != null && e.Button == MouseButtons.Left)
            {
                int newX = selectedPiece.Left + (e.X - originalPosition.X);
                int newY = selectedPiece.Top + (e.Y - originalPosition.Y);

                int gridX = (newX + pieceWidth / 2) / pieceWidth * pieceWidth;
                int gridY = (newY + pieceHeight / 2) / pieceHeight * pieceHeight;

                if (gridX < 0)
                    gridX = 0;
                else if (gridX + pieceWidth > pictureBoxStart.Width)
                    gridX = pictureBoxStart.Width - pieceWidth;

                if (gridY < 0)
                    gridY = 0;
                else if (gridY + pieceHeight > pictureBoxStart.Height)
                    gridY = pictureBoxStart.Height - pieceHeight;

                selectedPiece.Left = gridX;
                selectedPiece.Top = gridY;
            }
        }

        private void PuzzlePiece_MouseUp(object sender, MouseEventArgs e)
        {
            if (selectedPiece != null)
            {
                selectedPiece.Tag = new Point(selectedPiece.Left, selectedPiece.Top);
                selectedPiece = null;
            }
        }

        private void pictureBoxStart_Click(object sender, EventArgs e)
        {

        }

        private void textBoxRows_TextChanged(object sender, EventArgs e)
        {
            rows = Convert.ToInt32(textBoxRows.Text);
        }

        private void textBoxColums_TextChanged(object sender, EventArgs e)
        {
            columns = Convert.ToInt32(textBoxColums.Text);
        }

        private void pictureBoxPuzzlePieces_Click(object sender, EventArgs e)
        {

        }
    }
}
