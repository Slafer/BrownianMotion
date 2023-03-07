namespace Animator_circles
{
    public partial class Form1 : Form
    {
        private Animator a;
        public Form1()
        {
            InitializeComponent();
            a = new Animator(panel1.Size, panel1.CreateGraphics());
            a.Start();
        }

        private void panel1_Click(object sender, EventArgs e)
        {
            Point p = panel1.PointToClient(Cursor.Position);
            a.AddCircle(p);
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            a.Resize(panel1.Size,panel1.CreateGraphics());
            //a.Start();
        }
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            a.Close();
            base.OnClosing(e);
        }
        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}