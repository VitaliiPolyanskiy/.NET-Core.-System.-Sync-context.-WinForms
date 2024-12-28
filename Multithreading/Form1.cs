using System;
using System.Threading;
using System.Windows.Forms;

namespace Multithreading
{
    public partial class Form1 : Form
    {
        public SynchronizationContext uiContext;

        public Form1()
        {
            InitializeComponent();
            // Получим контекст синхронизации для текущего потока 
            uiContext = SynchronizationContext.Current;
        }

        private void ThreadFunk()
        {
            uiContext.Send(d => progressBar1.Minimum = 0, null);
            uiContext.Send(d => progressBar1.Maximum = (int)d, 230);
            uiContext.Send(d => progressBar1.Value = 0, null);
            uiContext.Send(d => button1.Enabled = false, null);

            for (int i = 0; i < 230; i++)
            {
                Thread.Sleep(50);
                // uiContext.Send отправляет синхронное сообщение в контекст синхронизации
                // SendOrPostCallback - делегат указывает метод, вызываемый при отправке сообщения в контекст синхронизации. 
                uiContext.Send(d => progressBar1.Value = (int)d /* Вызываемый делегат SendOrPostCallback */,
                    i /* Объект, переданный делегату */); // добавляем в список имя клиента
            }
            uiContext.Send(d => button1.Enabled = true, null);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Создание делегата функции, в которой будет работать новый поток
            ThreadStart MethodThread = new ThreadStart(ThreadFunk);
            // Создание объекта потока
            Thread thread = new Thread(MethodThread);
            thread.IsBackground = true;
            // Старт потока
            thread.Start();
        }
    }
}
