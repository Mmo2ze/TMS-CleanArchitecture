using MassTransit;
using TMS.MessagingContracts.Teacher;

namespace TMS.Conusmer.Consumers.Teachers;

public class TeacherPhoneChangedConsumer: IConsumer<TeacherPhoneChangedEvent>
{
    
    public Task Consume(ConsumeContext<TeacherPhoneChangedEvent> context)
    {
        Console.WriteLine("");    
        Console.WriteLine("--- TeacherPhoneChanged ---");
        Console.WriteLine($"--- TeacherId: {context.Message.Name} ---");
        Console.WriteLine($"--- Phone: {context.Message.TeacherPhone} ---");
        Console.WriteLine("");    
        return Task.CompletedTask;
    }
}