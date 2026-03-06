using DemoCRM.Application.Events;
using System.Text.Json;

namespace DemoCRM.Infrastructure.Messaging
{
    public class RabbitMqPublisher
    {
        private readonly EmailService _emailService;

        public RabbitMqPublisher(EmailService emailService)
        {
            _emailService = emailService;
        }

        public async Task PublishAsync(string eventType, string payload)
        {
            if (eventType == nameof(StudentCreatedEvent))
            {
                var studentEvent =  JsonSerializer.Deserialize<StudentCreatedEvent>(payload);

                await _emailService.SendStudentCreatedEmail(
                    studentEvent.Email,
                    studentEvent.Name
                );
            }
        }
    }
}
