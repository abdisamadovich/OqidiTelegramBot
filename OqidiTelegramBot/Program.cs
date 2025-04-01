using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

class Program
{
    private static ITelegramBotClient botClient;

    static async Task Main(string[] args)
    {
        // Bot tokeningizni shu yerga qo‘ying
        botClient = new TelegramBotClient("7874845783:AAHZewgaOkdDn7G3Yf1N-QogR9x8aZFSoG4");

        // CancellationToken yordamida botni to‘xtatish imkoniyati
        using var cts = new CancellationTokenSource();

        // Botni polling rejimida ishga tushirish
        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = Array.Empty<UpdateType>() // Barcha yangilanishlarni qabul qilish
        };

        botClient.StartReceiving(
            HandleUpdateAsync,
            HandleErrorAsync,
            receiverOptions,
            cts.Token
        );

        Console.WriteLine("Bot ishga tushdi. Chiqish uchun Ctrl+C bosing...");

        // Ctrl+C bosilguncha kutish
        Console.ReadLine();

        // Botni to‘xtatish
        cts.Cancel();
    }

    // Yangilanishlarni qayta ishlash
    private static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Type == UpdateType.Message && update.Message.Text != null)
        {
            var message = update.Message;
            Console.WriteLine($"Xabar keldi: {message.Text}");

            if (message.Text == "/start")
            {
                await botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: "Salom! Men C# da yozilgan Telegram botman. Menga xabar yuboring, men uni qaytaraman!",
                    cancellationToken: cancellationToken
                );
            }
            else
            {
                await botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: $"Siz yozdingiz: {message.Text}",
                    cancellationToken: cancellationToken
                );
            }
        }
    }

    // Xatolarni qayta ishlash
    private static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Xato yuz berdi: {exception.Message}");
        return Task.CompletedTask;
    }
}