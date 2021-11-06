namespace Friedforfun.SteeringBehaviours.Core
{
    public interface ICreateSteeringJob
    {
        public float[] GetContextMap();

        public void ScheduleJob();

        public void CompleteJob();
    }
}
