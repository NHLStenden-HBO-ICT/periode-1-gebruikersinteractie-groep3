using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;


public class TimerService
{
    private DispatcherTimer timer;
    private int timerDurationInMinutes; // De duur van de timer in minuten
    public event Action TimerExpired;

    public TimerService(int durationInMinutes)
    {
        timerDurationInMinutes = durationInMinutes;

        timer = new DispatcherTimer();
        timer.Interval = TimeSpan.FromMinutes(1); // Timer interval van 1 minuut
        timer.Tick += Timer_Tick;
        timer.Start();
    }

    private void Timer_Tick(object sender, EventArgs e)
    {
        if (timerDurationInMinutes > 0)
        {
            timerDurationInMinutes--;
        }
        else
        {
            // Timer is afgelopen, vuur het event af
            TimerExpired?.Invoke();
        }
    }

    public void StopTimer()
    {
        timer.Stop();
    }

}



