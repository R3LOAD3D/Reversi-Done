using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication4
{
    public partial class Form1 : Form
    {
        int numvak = 10;  // number of squares in the grid, both in x and y-direction
        int gridvaksize = 60; //size of each gridsquare
        string speleraanzet = "Blauw";
        int teller1;
        int teller2;
        int[,] tegels;



        bool helpfunctie = false;

        public Form1()
        {
            InitializeComponent();

            //abonnementen op events

            panel1.Paint += new PaintEventHandler(panel1_Paint);
            panel3.Paint += new PaintEventHandler(panel3_Paint);
            panel1.MouseClick += panel1_MouseClick;
            tegels = new int[numvak, numvak];
            tegels[numvak / 2, numvak / 2] = 1;
            tegels[numvak / 2, numvak / 2 - 1] = -1;
            tegels[numvak / 2 - 1, numvak / 2 - 1] = 1;
            tegels[numvak / 2 - 1, numvak / 2] = -1;
            panel1.Size = new System.Drawing.Size(numvak * gridvaksize, numvak * gridvaksize);

        }


        private bool ValidMove(int MouseX, int MouseY, int PlayerColour) // Checks if a move is valid 
        {
            if (tegels[MouseX, MouseY] == 0)
            {
                for (int dx = -1; dx < 2; dx++)
                {
                    if (MouseX + dx < 0 || MouseX + dx > numvak)
                        continue;
                    for (int dy = -1; dy < 2; dy++) 
                    {
                        if (MouseY + dy < 0 || MouseY + dy > numvak)
                            continue;
                        try
                        {
                            int offset = 1;
                            while (tegels[MouseX + dx * offset, MouseY + dy * offset] == -PlayerColour)
                            {
                                offset++;

                            }

                            if (offset == 1)
                            {
                                continue;

                            }
                            if (tegels[MouseX + dx * offset, MouseY + dy * offset] == PlayerColour)
                            {

                                return true;
                            }

                        }
                        catch (Exception)
                        {


                        }

                    }

                }


            }
            return false;
        }

        private void CheckMovesLeft() // Checks how many moves are left in the grid based on numvak
        {
            bool CanBlueMove = false;
            bool CanRedMove = false;

            for (int xgrid = 0; xgrid < numvak; xgrid++)
            {
                for (int ygrid = 0; ygrid < numvak; ygrid++)
                {
                    if (ValidMove(xgrid, ygrid, 1))
                        CanBlueMove = true;
                    if (ValidMove(xgrid, ygrid, -1))
                        CanRedMove = true;
                }

            }

            if (!CanBlueMove && !CanRedMove)
                winner();

            if (!CanBlueMove && CanRedMove)
                speleraanzet = "Rood";

            if (CanBlueMove && !CanRedMove)
                speleraanzet = "Blauw";

        }
        void panel1_MouseClick(object sender, MouseEventArgs e) // When the mouse has been clicked it will draw an ellipse based on who has the turn
        {
            int MouseX = e.X / gridvaksize; //X-coördinates mouse
            int MouseY = e.Y / gridvaksize; //Y-coördinates mouse

            Graphics gr = this.panel1.CreateGraphics();
            if (speleraanzet == "Blauw")
            {
                if (ValidMove(MouseX, MouseY, 1))
                {
                    tegels[MouseX, MouseY] = 1;
                    ColourChanger(1, MouseX, MouseY);
                    panel1.Invalidate();
                    speleraanzet = "Rood";
                }

            }
            else
            {
                if (ValidMove(MouseX, MouseY, -1))
                {
                    tegels[MouseX, MouseY] = -1;
                    ColourChanger(-1, MouseX, MouseY);
                    panel1.Invalidate();
                    speleraanzet = "Blauw";
                }
            }


            Label1_Teller();
            TextBox3_Speler();
            CheckMovesLeft();
        }

        private void ColourChanger(int PlayerColour, int GridX, int GridY) //Changes the colour of the encapsulated circles
        {
            int EnemyColour = -PlayerColour;
            for (int dx = -1; dx < 2; dx++)
            {
                for (int dy = -1; dy < 2; dy++)
                {
                    int offset = 1;
                    try
                    {
                        while (tegels[GridX + dx * offset, GridY + dy * offset] == EnemyColour)
                        {
                            offset++;

                        }


                        if (tegels[GridX + dx * offset, GridY + dy * offset] == PlayerColour)
                        {

                            for (int teller = 1; teller <= offset; teller++)
                            {
                                tegels[GridX + dx * teller, GridY + dy * teller] = PlayerColour;
                            }

                        }
                    }
                    catch (Exception)
                    {

                    }

                }

            }


        }


        private void panel1_Paint(object obj, PaintEventArgs e) // Drawing of the grid
        {
            Pen DrawPen = new Pen(Color.Black);
            DrawPen.Width = 3;



            e.Graphics.FillEllipse(Brushes.Blue, numvak / 2 * gridvaksize, numvak / 2 * gridvaksize, gridvaksize, gridvaksize);
            e.Graphics.FillEllipse(Brushes.Blue, numvak / 2 * gridvaksize - gridvaksize, numvak / 2 * gridvaksize - gridvaksize, gridvaksize, gridvaksize);
            e.Graphics.FillEllipse(Brushes.Red, numvak / 2 * gridvaksize, numvak / 2 * gridvaksize - gridvaksize, gridvaksize, gridvaksize);
            e.Graphics.FillEllipse(Brushes.Red, numvak / 2 * gridvaksize - gridvaksize, numvak / 2 * gridvaksize, gridvaksize, gridvaksize);
            for (int y = 0; y <= numvak; y++)
            {
                e.Graphics.DrawLine(DrawPen, y * gridvaksize, 0, y * gridvaksize, numvak * gridvaksize);  //Vertical Grid Lines
                e.Graphics.DrawLine(DrawPen, 0, y * gridvaksize, gridvaksize * numvak, y * gridvaksize); // Horizontal Grid Lines

            }

            if (helpfunctie)
                Helpfunctie(); ;

            for (int xgrid = 0; xgrid < numvak; xgrid++)
            {
                for (int ygrid = 0; ygrid < numvak; ygrid++)
                {

                    if (tegels[xgrid, ygrid] == 1)
                    {
                        e.Graphics.FillEllipse(Brushes.Blue, xgrid * gridvaksize, ygrid * gridvaksize, gridvaksize, gridvaksize); // Creates the blue ellipse

                    }
                    if (tegels[xgrid, ygrid] == -1)
                    {
                        e.Graphics.FillEllipse(Brushes.Red, xgrid * gridvaksize, ygrid * gridvaksize, gridvaksize, gridvaksize); // Creates the red ellipse
                    }
                    if (tegels[xgrid, ygrid] == 2)
                    {
                        e.Graphics.DrawEllipse(DrawPen, xgrid * gridvaksize + gridvaksize / 4, ygrid * gridvaksize + gridvaksize / 4, gridvaksize / 2, gridvaksize / 2); //Creates the ellipse for the help-function, but turns the value to 0, so it can be filled with other ellipses.
                        tegels[xgrid, ygrid] = 0;
                    }
                }
            }

        }

        private void button1_Click(object sender, EventArgs e) //New game
        {
            for (int x = 0; x < numvak; x++)
            {
                for (int y = 0; y < numvak; y++)
                {
                    tegels[x, y] = 0;
                }

            }
            speleraanzet = "Blauw";

            panel1.Invalidate();
            label1.Text = "2";
            label2.Text = "2";
            TextBox3_Speler();

        }

        private void Helpfunctie() //Helpfunctie methode
        {
            int spelerkleur;
            if (speleraanzet == "Rood")
                spelerkleur = -1;
            else
                spelerkleur = 1;
            for (int x = 0; x < numvak; x++)
            {
                for (int y = 0; y < numvak; y++)
                {
                    if (ValidMove(x, y, spelerkleur))
                    {
                        tegels[x, y] = 2;
                    }
                }
            }

        }


        private void button3_Click(object sender, EventArgs e) //Help
        {
            helpfunctie = !helpfunctie;
            panel1.Invalidate();
        }

        private void winner() // Winner-Box
        {
            if (teller1 > teller2)
            {
                textBox4.Text = "Rood wint!";
                textBox4.BackColor = Color.Red;
            }
            else if (teller1 < teller2)
            {
                textBox4.Text = "Blauw wint";
                textBox4.BackColor = Color.Blue;
            }
            else
                textBox4.Text = "Gelijk";
        }


        private void panel1_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e) //panel-positie voor tussenstand rode cirkels
        {
            e.Graphics.FillEllipse(Brushes.Red, 0, 0, 50, 50);
        }

        private void panel4_Paint(object sender, PaintEventArgs e) //panel-positie voor tussenstand blauwe cirkels 
        {
            e.Graphics.FillEllipse(Brushes.Blue, 0, 0, 50, 50);
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            Invalidate();
        }

        private void TextBox3_Speler()
        {
            string SpelerX = speleraanzet;
            textBox3.Text = SpelerX;
        }

        private void Label1_Teller() // Counter Blue & Red Cirkels
        {
            teller1 = 0;
            teller2 = 0;
            for (int x = 0; x < numvak; x++)
            {
                for (int y = 0; y < numvak; y++)
                {
                    if (tegels[x, y] == 1)
                    {
                        teller2++;

                    }
                    if (tegels[x, y] == -1)
                    {
                        teller1++;
                    }
                }
            }
            label1.Text = teller1.ToString();
            label2.Text = teller2.ToString();

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }


    }
}
