﻿using System;
using System.Windows.Forms;

namespace WindowsFormsAppProxy
{
    public partial class Form1 : Form
    {
        private readonly Simplex _client;

        public Form1()
        {
            _client = new Simplex();
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var a1 = new A { s = S1.Text, k = Convert.ToInt32(K1.Text), f = float.Parse(F1.Text) };
            var a2 = new A { s = S2.Text, k = Convert.ToInt32(K2.Text), f = float.Parse(F2.Text) };
            MessageBox.Show(a1.f.ToString());
            MessageBox.Show(a2.f.ToString());
            var res = _client.Sum(a1, a2);

            S3.Text = res.s;
            K3.Text = res.k.ToString();
            F3.Text = res.f.ToString();
        }


    }
}
