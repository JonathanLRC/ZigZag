using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZigZag
{
    public partial class Form1 : Form
    {
        Label[] Columna1 = new System.Windows.Forms.Label[7];
        Label[] Columna2 = new System.Windows.Forms.Label[7];
        Color[] Colores = new Color[10];
        Random r = new Random();
        System.Media.SoundPlayer sp = new System.Media.SoundPlayer(@"..\..\..\Musica.wav");
        int limite = 4;
        int score = 0;
        int turno = 1;
        bool gameOver = false;
        int[][] saved = new int[3][];
        labelselec selected;
        struct labelselec
        {
            public int columna;
            public int i;
            public int value;
            public bool activa;
        }

        public Form1()
        {
            InitializeComponent();
            InitializeColors();
            selected.activa = false;
            for(int i = 0; i < 3; i++)
            {
                saved[i] = new int[7];
            }
        }
        private void InitializeColors()
        {
            Colores[0] = Color.LightSeaGreen;
            Colores[1] = Color.FromArgb(234, 131, 132);
            Colores[2] = Color.FromArgb(70, 142, 244);
            Colores[3] = Color.FromArgb(122, 204, 122);
            Colores[4] = Color.FromArgb(245, 162, 84);
            Colores[5] = Color.FromArgb(224, 111, 225);
            Colores[6] = Color.FromArgb(62, 204, 205);
            Colores[7] = Color.FromArgb(242, 217, 40);
            Colores[8] = Color.FromArgb(203, 153, 116);
            Colores[9] = Color.DarkBlue;
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            sp.PlayLooping();
            limite = 4;
            pictureBox1.Hide();
            pictureBox2.Show();
            pictureBox3.Show();
            pictureBox4.Hide();
            pictureBox5.Hide();
            LeftArrow.Show();
            label1.Show();
            label4.Show();
            Next.Show();
            RightArrow.Hide();
            LeftArrow.Show();
            Undo.Show();
            label1.Text = "0";
            turno = 1;
            score = 0;
            for (int i = 0; i < 7; i++)
            {
                Columna1[i] = new Label();
                Columna1[i].Name = i.ToString();
                Columna1[i].Size = new Size(60, 30);
                Columna1[i].BackColor = Color.FromArgb(230, 230, 230);
                Columna1[i].TextAlign = ContentAlignment.MiddleCenter;
                Columna1[i].Font = new Font("Microsoft Sans Serif", 16);
                Columna1[i].ForeColor = Color.White;
                Columna1[i].Location = new System.Drawing.Point(70, i * 35 + 120);
                Columna1[i].Click += Form1_Click;
                this.Controls.Add(Columna1[i]);
            }
            Columna1[0].Location = new System.Drawing.Point(70, 95);

            for (int i = 0; i < 7; i++)
            {
                Columna2[i] = new Label();
                Columna2[i].Name = i.ToString();
                Columna2[i].Size = new Size(60, 30);
                Columna2[i].BackColor = Color.FromArgb(230, 230, 230);
                Columna2[i].TextAlign = ContentAlignment.MiddleCenter;
                Columna2[i].Font = new Font("Microsoft Sans Serif", 16);
                Columna2[i].ForeColor = Color.White;
                Columna2[i].Location = new System.Drawing.Point(170, i * 35 + 120);
                Columna2[i].Click += Form2_Click;
                this.Controls.Add(Columna2[i]);
            }
            Columna2[0].Location = new System.Drawing.Point(170, 95);

            Columna1[1].BackColor = Columna2[1].BackColor = Color.FromArgb(255, 215, 215);

            Columna1[4].Text = Columna1[6].Text = r.Next(1, limite).ToString();
            Columna1[4].BackColor = Columna1[6].BackColor = Colores[int.Parse(Columna1[4].Text) % 10];
            int ran = r.Next(1, limite);
            while (ran == int.Parse(Columna1[4].Text))  ran = r.Next(1, limite);
            Columna1[5].Text = ran.ToString();
            Columna1[5].BackColor = Colores[int.Parse(Columna1[5].Text) % 10];
            Columna2[4].Text = Columna2[6].Text = r.Next(1, limite).ToString();
            Columna2[4].BackColor = Columna2[6].BackColor = Colores[int.Parse(Columna2[4].Text) % 10];
            while (ran == int.Parse(Columna2[4].Text))  ran = r.Next(1, limite);
            Columna2[5].Text = ran.ToString();
            Columna2[5].BackColor = Colores[int.Parse(Columna2[5].Text) % 10];

            Columna1[0].Text = r.Next(1, limite).ToString();
            Columna1[0].BackColor = Colores[int.Parse(Columna1[0].Text) % 10];
            Columna2[0].Text = r.Next(1, limite).ToString();
            Columna2[0].BackColor = Colores[int.Parse(Columna2[0].Text) % 10];
            Next.Text = r.Next(1, limite).ToString();
            Next.BackColor = Colores[int.Parse(Next.Text) % 10];
            for(int i = 0; i < 3; i++)
            {
                for(int j = 0; j < 7; j++)
                {
                    saved[i][j] = 0;
                }
            }
            Save();
            gameOver = false;
        }
        private void Form2_Click(object sender, EventArgs e)
        {
            if (gameOver) return;
            Label selec = sender as Label;
            if(selec.Name == "0")
            {
                if (turno != 2) return;
                Save();
                Columna2[1].Text = Columna2[0].Text;
                Columna2[1].BackColor = Columna2[0].BackColor;
                Columna2[0].Text = r.Next(1, limite).ToString();
                Columna2[0].BackColor = Colores[int.Parse(Columna2[0].Text) % 10];
                Next.Text = r.Next(1, limite).ToString();
                Next.BackColor = Colores[int.Parse(Next.Text) % 10];
                turno = 4;
                GameUpdate();
                return;
            }
            if (selec.Text == "")
            {
                if (!selected.activa || int.Parse(selec.Name) < 1) return;
                if (selected.columna != 2)
                {
                    Save();
                    int i = 2;
                    while (i < 7 && Columna2[i].Text == "") i++;
                    i--;
                    Columna2[i].Text = selected.value.ToString();
                    Columna2[i].BackColor = Colores[selected.value % 10];
                    unSelect();
                    Reset(selected.columna, selected.i);
                    selected.activa = false;
                }
                GameUpdate();
                return;
            }
            selec.Size = new Size(66, 36);
            selec.Location = new System.Drawing.Point(selec.Location.X - 3, selec.Location.Y - 3);
            if (selected.activa)
            {
                if (selected.value != int.Parse(selec.Text) || selected.columna == 2)
                {
                    selec.Size = new Size(60, 30);
                    selec.Location = new System.Drawing.Point(170, int.Parse(selec.Name) * 35 + 120);
                    unSelect();
                    return;
                }
                Save();
                selec.Text = (selected.value + 1).ToString();
                selec.BackColor = Colores[int.Parse(selec.Text) % 10];
                score += (int)Math.Pow(2, int.Parse(selec.Text));
                label1.Text = score.ToString();
                selec.Size = new Size(60, 30);
                selec.Location = new System.Drawing.Point(170, int.Parse(selec.Name) * 35 + 120);
                unSelect();
                Reset(selected.columna, selected.i);
                selected.activa = false;
                GameUpdate();
                if (selec.Text != "" && int.Parse(selec.Text) > limite)
                    limite = int.Parse(selec.Text);
            }
            else
            {
                selected.columna = 2;
                selected.i = int.Parse(selec.Name);
                selected.value = int.Parse(selec.Text);
                selected.activa = true;
            }
        }

        private void Form1_Click(object sender, EventArgs e)
        {
            if (gameOver) return;
            Label selec = sender as Label;
            if(selec.Name == "0")
            {
                if (turno != 1) return;
                Save();
                Columna1[1].Text = Columna1[0].Text;
                Columna1[1].BackColor = Columna1[0].BackColor;
                Columna1[0].Text = r.Next(1, limite).ToString();
                Columna1[0].BackColor = Colores[int.Parse(Columna1[0].Text) % 10];
                Next.Text = r.Next(1, limite).ToString();
                Next.BackColor = Colores[int.Parse(Next.Text) % 10];
                turno = 3;
                GameUpdate();
                return;
            }
            if (selec.Text == "")
            {
                if (!selected.activa || int.Parse(selec.Name) < 1) return;
                if (selected.columna != 1)
                {
                    Save();
                    int i = 2;
                    while (i < 7 && Columna1[i].Text == "") i++;
                    i--;
                    Columna1[i].Text = selected.value.ToString();
                    Columna1[i].BackColor = Colores[selected.value % 10];
                    unSelect();
                    Reset(selected.columna, selected.i);
                    selected.activa = false;
                }
                GameUpdate();
                return;
            }
            selec.Size = new Size(66, 36);
            selec.Location = new System.Drawing.Point(selec.Location.X - 3, selec.Location.Y - 3);
            if (selected.activa)
            {
                if (selected.value != int.Parse(selec.Text) || selected.columna == 1)
                {
                    selec.Size = new Size(60, 30);
                    selec.Location = new System.Drawing.Point(70, int.Parse(selec.Name) * 35 + 120);
                    unSelect();
                    return;
                }
                Save();
                selec.Text = (selected.value + 1).ToString();
                selec.BackColor = Colores[int.Parse(selec.Text) % 10];
                score += (int)Math.Pow(2, int.Parse(selec.Text));
                label1.Text = score.ToString();
                selec.Size = new Size(60, 30);
                selec.Location = new System.Drawing.Point(70, int.Parse(selec.Name) * 35 + 120);
                unSelect();
                Reset(selected.columna, selected.i);
                selected.activa = false;
                GameUpdate();
                if (selec.Text != "" && int.Parse(selec.Text) > limite)
                    limite = int.Parse(selec.Text);
            }
            else
            {
                selected.columna = 1;
                selected.i = int.Parse(selec.Name);
                selected.value = int.Parse(selec.Text);
                selected.activa = true;
            }
        }
        private void Reset(int Columna, int i)
        {
            if (Columna == 1)
            {
                Columna1[i].Size = new Size(60, 30);
                Columna1[i].BackColor = Color.FromArgb(230, 230, 230);
                Columna1[i].Text = "";
                Columna1[i].Location = new System.Drawing.Point(70, i * 35 + 120);
                if (i == 1) Columna1[i].BackColor = Color.FromArgb(255, 215, 215);
            }
            else
            {
                Columna2[i].Size = new Size(60, 30);
                Columna2[i].BackColor = Color.FromArgb(230, 230, 230);
                Columna2[i].Text = "";
                Columna2[i].Location = new System.Drawing.Point(170, i * 35 + 120);
                if (i == 1) Columna2[i].BackColor = Color.FromArgb(255, 215, 215);
            }
        }
        private void unSelect()
        {
            if(selected.columna == 1)
            {
                Columna1[selected.i].Size = new Size(60, 30);
                Columna1[selected.i].Location = new System.Drawing.Point(70, selected.i * 35 + 120);
            }
            else
            {
                Columna2[selected.i].Size = new Size(60, 30);
                Columna2[selected.i].Location = new System.Drawing.Point(170, selected.i * 35 + 120);
            }
            selected.activa = false;
        }
        private void GameUpdate()
        {
            Recorrer();
            for(int i = 1; i < 6; i++)
            {
                if(Columna1[i].Text != "" && Columna1[i].Text == Columna1[i + 1].Text)
                {
                    Columna1[i + 1].Text = (int.Parse(Columna1[i+1].Text) + 1).ToString();
                    Columna1[i + 1].BackColor = Colores[int.Parse(Columna1[i + 1].Text) % 10];
                    score += (int) Math.Pow(2, int.Parse(Columna1[i + 1].Text));
                    label1.Text = score.ToString();
                    Reset(1, i);
                    Recorrer();
                    if (Columna1[i + 1].Text != "" && int.Parse(Columna1[i + 1].Text) > limite)
                        limite = int.Parse(Columna1[i + 1].Text);
                }
            }
            for (int i = 1; i < 6; i++)
            {
                if (Columna2[i].Text != "" && Columna2[i].Text == Columna2[i + 1].Text)
                {
                    Columna2[i + 1].Text = (int.Parse(Columna2[i + 1].Text) + 1).ToString();
                    Columna2[i + 1].BackColor = Colores[int.Parse(Columna2[i + 1].Text) % 10];
                    score += (int)Math.Pow(2, int.Parse(Columna2[i + 1].Text));
                    label1.Text = score.ToString();
                    Reset(2, i);
                    Recorrer();
                    if (Columna2[i + 1].Text != "" && int.Parse(Columna2[i + 1].Text) > limite)
                        limite = int.Parse(Columna2[i + 1].Text);
                }
            }
            Recorrer();
            if (turno == 1)
            {
                Columna1[1].Text = Columna1[0].Text;
                Columna1[1].BackColor = Columna1[0].BackColor;
                Columna1[0].Text = Next.Text;
                Columna1[0].BackColor = Next.BackColor;
                Next.Text = r.Next(1, limite).ToString();
                Next.BackColor = Colores[int.Parse(Next.Text) % 10];
                turno = 3;
                GameUpdate();
            }else if(turno == 2)
            {
                Columna2[1].Text = Columna2[0].Text;
                Columna2[1].BackColor = Columna2[0].BackColor;
                Columna2[0].Text = Next.Text;
                Columna2[0].BackColor = Next.BackColor;
                Next.Text = r.Next(1, limite).ToString();
                Next.BackColor = Colores[int.Parse(Next.Text) % 10];
                turno = 4;
                GameUpdate();
            }else if(turno == 3)
            {
                turno = 2;
                RightArrow.Show();
                LeftArrow.Hide();
            }
            else if(turno == 4)
            {
                turno = 1;
                RightArrow.Hide();
                LeftArrow.Show();
            }

            if(Columna1[1].Text != "" || Columna2[1].Text != "")
            {
                if (score > int.Parse(label3.Text)) label3.Text = score.ToString();
                if (!gameOver)
                {
                    MessageBox.Show("Juego Terminado!");
                    Restart();
                }
                gameOver = true;
            }
        }
        private void Recorrer()
        {
            int r = 0;
            if (Columna1[2].Text == "" && Columna1[1].Text != "")
            {
                Columna1[2].Text = Columna1[1].Text;
                Columna1[2].BackColor = Columna1[1].BackColor;
                Reset(1, 1);
                r++;
            }
            for (int i = 2; i < 6; i++)
            {
                if (Columna1[i + 1].Text == "" && Columna1[i].Text != "")
                {
                    Columna1[i + 1].Text = Columna1[i].Text;
                    Columna1[i + 1].BackColor = Columna1[i].BackColor;
                    Reset(1, i);
                    r++;
                }
            }
            if (Columna2[2].Text == "" && Columna2[1].Text != "")
            {
                Columna2[2].Text = Columna2[1].Text;
                Columna2[2].BackColor = Columna2[1].BackColor;
                Reset(2, 1);
                r++;
            }
            for (int i = 2; i < 6; i++)
            {
                if (Columna2[i + 1].Text == "" && Columna2[i].Text != "")
                {
                    Columna2[i + 1].Text = Columna2[i].Text;
                    Columna2[i + 1].BackColor = Columna2[i].BackColor;
                    Reset(2, i);
                    r++;
                }
            }
            if (r > 0) Recorrer();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Restart();
        }

        private void Restart()
        {
            pictureBox2.Hide();
            pictureBox3.Hide();
            pictureBox4.Show();
            //pictureBox5.Show();
            Undo.Hide();
            label1.Hide();
            label4.Hide();
            Next.Hide();
            RightArrow.Hide();
            LeftArrow.Hide();
            for (int i = 0; i < 7; i++)
            {
                this.Controls.Remove(Columna1[i]);
                this.Controls.Remove(Columna2[i]);
            }
            pictureBox1.Show();
        }
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            limite = 4;
            turno = 1;
            RightArrow.Hide();
            LeftArrow.Show();
            Undo.Show();
            for (int i = 1; i < 7; i++)
            {
                Reset(1, i);
                Reset(2, i);
            }

            Columna1[0].Location = new System.Drawing.Point(70, 95);
            Columna2[0].Location = new System.Drawing.Point(170, 95);

            Columna1[4].Text = Columna1[6].Text = r.Next(1, limite).ToString();
            Columna1[4].BackColor = Columna1[6].BackColor = Colores[int.Parse(Columna1[4].Text) % 10];
            int ran = r.Next(1, limite);
            while (ran == int.Parse(Columna1[4].Text)) ran = r.Next(1, limite);
            Columna1[5].Text = ran.ToString();
            Columna1[5].BackColor = Colores[int.Parse(Columna1[5].Text) % 10];
            Columna2[4].Text = Columna2[6].Text = r.Next(1, limite).ToString();
            Columna2[4].BackColor = Columna2[6].BackColor = Colores[int.Parse(Columna2[4].Text) % 10];
            while (ran == int.Parse(Columna2[4].Text)) ran = r.Next(1, limite);
            Columna2[5].Text = ran.ToString();
            Columna2[5].BackColor = Colores[int.Parse(Columna2[5].Text) % 10];

            Columna1[0].Text = r.Next(1, limite).ToString();
            Columna1[0].BackColor = Colores[int.Parse(Columna1[0].Text) % 10];
            Columna2[0].Text = r.Next(1, limite).ToString();
            Columna2[0].BackColor = Colores[int.Parse(Columna2[0].Text) % 10];
            Next.Text = r.Next(1, limite).ToString();
            Next.BackColor = Colores[int.Parse(Next.Text) % 10];
            label1.Text = "0";
            score = 0;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    saved[i][j] = 0;
                }
            }
            Save();
            gameOver = false;
        }

        private void Undo_Click(object sender, EventArgs e)
        {
            Undo.Hide();
            for (int i = 0; i < 7; i++)
            {
                if (saved[1][i] == 0)
                    Reset(1, i);
                else
                {
                    Columna1[i].Text = saved[1][i].ToString();
                    Columna1[i].BackColor = Colores[saved[1][i] % 10];
                }

                if (saved[2][i] == 0)
                    Reset(2, i);
                else
                {
                    Columna2[i].Text = saved[2][i].ToString();
                    Columna2[i].BackColor = Colores[saved[2][i] % 10];
                }

                
                Next.Text = saved[0][0].ToString();
                Next.BackColor = Colores[saved[0][0] % 10];
                turno = saved[0][1];
                score = saved[0][2];
            }
            if(turno == 1)
            {
                RightArrow.Hide();
                LeftArrow.Show();
            }else
            {
                RightArrow.Show();
                LeftArrow.Hide();
            }
            label1.Text = score.ToString();
        }

        private void Save()
        {
            for (int i = 0; i < 7; i++)
            {
                if (Columna1[i].Text != "")
                    saved[1][i] = int.Parse(Columna1[i].Text);
                else
                    saved[1][i] = 0;
                if (Columna2[i].Text != "")
                    saved[2][i] = int.Parse(Columna2[i].Text);
                else
                    saved[2][i] = 0;

                saved[0][0] = int.Parse(Next.Text);
                saved[0][1] = turno;
                saved[0][2] = score;
            }
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            sp.Stop();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            MessageBox.Show("No disponible");
        }
    }
}
