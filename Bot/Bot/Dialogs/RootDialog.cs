using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Bot_Application2;

namespace Bot.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public static List<Problem> todo;
        private List<int> CheckCitats = new List<int>();
        public static int timeseed;
        private List<string> citats = new List<string>{
        "Это время, как и любое другое, очень удачное, если мы знаем, что с ними делать. Ральф Вальдо Эмерсон",
        "Жить жизнью не имея плана, это все равно что смотреть телевизор, когда пульт находится в чужих руках. Питер Турла",
        "Любой тайм-менеджмент начинается с планирования. Том Грининг",
        "Время — это то, чего мы хотим больше всего и то, что мы хуже всего умеем использовать. Вильям Пенн",
        "Большинство людей путает неумение распоряжаться временем с судьбой. Кин Хаббард",
        "Делать две вещи одновременно, означает не сделать ни одной. Публиус Сирус",
        "Те, хуже всего умеют распоряжаться своим временем, зачастую жалуются, что его не хватает. Ла Бруйер",
        "Время — дефицитный ресурс и если им не уметь управлять, то ничем не сможем управлять. Питер Ф. Драккер",
        "Самый надежный способ прибыть поздно — иметь много времени. Лео Кеннеди",
        "Ничто не может быть сложным, если разбить задачу на мелкие части. Генри Форд",
        "Не ждите. Время никогда не будет подходящим. Наполеон Хилл",
        "Эффективный менеджмент всегда подразумевает правильно заданный вопрос. Роберт Хеллер",
        "Лучше на 3 часа раньше, чем на одну минуту позже. Вильям Шекспир",
        "Богатство подразумевает наличие денег, состоятельность — наличие времени. Маргарет Боннано",
        "Время для действий — сейчас. Никогда не поздно сделать что-нибудь. Антуан де Сент-Экзюпери",
        "Не задерживайтесь в прошлом, не мечтайте о будущем, сосредоточьте разум на настоящем. Будда",
        "Если люди не смеются над вашими целями, значит ваши цели слишком мелкие. Азим Премжи",
        "Пробуйте и терпите неудачу, но не прерывайте ваших стараний. Стивен Каггва",
        "К черту все! Берись и делай! Ричард Брэнсон",
        "Мы сами должны стать теми переменами, которые хотим видеть в мире. Махатма Ганди",
        "Препятствия – это те страшные вещи, которые вы видите, когда отводите глаза от цели. Генри Форд",
        "Постановка целей является первым шагом на пути превращения мечты в реальность. Тони Роббинс",
        "Быть самым богатым человеком на кладбище для меня не важно… Ложиться спать и говорить себе, что сделал действительно нечто прекрасное, - вот что важно! Стив Джобс",
        "Когда вы знаете, чего хотите, и вы хотите этого достаточно сильно, вы найдете способ получить это. Джим Рон",
        "Я трачу почти все свое время на Facebook. У меня практически нет времени на новые увлечения. Поэтому я ставлю перед собой четкие цели. Марк Цукенберг",
        "Чтобы достичь поставленных целей, нужны терпение и энтузиазм. Мыслите глобально – но будьте реалистами. Дональд Трамп",
        "Самый опасный яд – чувство достижения цели. Противоядие к которому – каждый вечер думать, что можно завтра сделать лучше. Ингвар Кампрад",
        "Пуля, просвистевшая на дюйм от цели, так же бесполезна, как та, что не вылетала из дула. Фенимор Купер",
        "Никогда, никогда не позволяйте другим убедить вас, что что-то сложно или невозможно. Дуглас Бадлер",
        "Успех обычно приходит к тем, кто слишком занят, чтобы его просто ждать. Генри Девид Торо",
        "Вдохновение приходит только во время работы. Габриэль Маркес",
        "Самая тяжелая часть работы — решиться приступить к ней. Габриэль Лауб",
        };
        bool taskadded;

        public Task StartAsync(IDialogContext context)
        {
            for (int i = 0; i < 32; i++)
                CheckCitats.Add(0);
            todo = new List<Problem>();
            context.PostAsync("Привет!");
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {

            PromptDialog.Choice(
            context: context,
            resume: SecondReceivedAsync,
            options: new List<string> { "Добавить задачу", "Посмотреть задачи на сегодня", "Удалить задачу", "Мотивирующая цитата на каждый день", "Настроить напоминание задач" },
            prompt: $"Привет! Хочешь добавить задачу на сегодня",
            promptStyle: PromptStyle.Auto);

        }

        private async Task SecondReceivedAsync(IDialogContext context, IAwaitable<string> result)
        {
            string input = await result;
            if (input == "Добавить задачу")
            {
                await context.PostAsync("Напиши свою задачу:");
                context.Wait(AddToList);
            }
            else if (input == "Посмотреть задачи на сегодня")
            {
                int number = 1;
                if (todo.Count == 0)
                {
                    await context.PostAsync("Задач нет");
                }
                else
                {
                    string tasks = "";
                    foreach (Problem task in todo)
                    {
                        tasks += String.Format("{0}) {1}", number.ToString(), task.Name);
                        tasks += "\n";

                        number++;
                    }
                    await context.PostAsync(tasks);
                }
                PromptDialog.Choice(
                context: context,
                resume: SecondReceivedAsync,
                options: new List<string> { "Добавить задачу", "Посмотреть задачи на сегодня", "Удалить задачу", "Мотивирующая цитата на каждый день", "Настроить напоминание задач" },
                prompt: $"Привет! Хочешь добавить задачу на сегодня",
                promptStyle: PromptStyle.Auto);
            }
            else if (input == "Удалить задачу")
            {
                if (todo.Count > 0)
                {
                    PromptDialog.Choice(
                    context: context,
                    resume: DeleteRecievedMessage,
                    options: new List<string> { "Удалить одну задачу", "Удалить все задачи", "Удалить задачи за последние сутки" },
                    prompt: $"Удаление задач",
                    promptStyle: PromptStyle.Auto);
                }
                else
                {
                    await context.PostAsync("Задач нет");
                    PromptDialog.Choice(
                    context: context,
                    resume: SecondReceivedAsync,
                    options: new List<string> { "Добавить задачу", "Посмотреть задачи на сегодня", "Удалить задачу", "Мотивирующая цитата на каждый день", "Настроить напоминание задач" },
                    prompt: $"Привет! Хочешь добавить задачу на сегодня",
                    promptStyle: PromptStyle.Auto);
                }
            }
            else if (input == "Мотивирующая цитата на каждый день")
            {
                if (!CheckCitats.Contains(0))
                    for (int i = 0; i < 32; i++)
                        CheckCitats[i] = 0;


                Random number = new Random();
                int ind = 0;
                do
                {
                    ind = number.Next(0, 32);
                    //await context.PostAsync(ind.ToString());
                }
                while (CheckCitats[ind] == 1);
                CheckCitats[ind] = 1;
                await context.PostAsync(citats[ind]);

                PromptDialog.Choice(
                context: context,
                resume: SecondReceivedAsync,
                options: new List<string> { "Добавить задачу", "Посмотреть задачи на сегодня", "Удалить задачу", "Мотивирующая цитата на каждый день", "Настроить напоминание задач" },
                prompt: $"Привет! Хочешь добавить задачу на сегодня",
                promptStyle: PromptStyle.Auto);
            }
            else if (input == "Настроить напоминание задач")
            {

                if (todo.Count > 0)
                {
                    PromptDialog.Text(
                    context: context,
                    resume: TextReceivedAsync,
                    prompt: "Введите в формате \"HH:MM:SS\"время, через которое будет происходить напоминание:"); ;
                }
                else
                {
                    await context.PostAsync("Задач нет");
                    PromptDialog.Choice(
                    context: context,
                    resume: SecondReceivedAsync,
                    options: new List<string> { "Добавить задачу", "Посмотреть задачи на сегодня", "Удалить задачу", "Мотивирующая цитата на каждый день", "Настроить напоминание задач" },
                    prompt: $"Привет! Хочешь добавить задачу на сегодня",
                    promptStyle: PromptStyle.Auto);
                }
            }
        }
        private async Task DeleteRecievedMessage(IDialogContext context, IAwaitable<string> result)
        {
            string input = await result;
            if (input == "Удалить одну задачу")
            {
                if (todo.Count > 0)
                {
                    int number = 1;
                    string tasks = "";
                    foreach (Problem task in todo)
                    {
                        tasks += String.Format("{0}) {1}", number.ToString(), task.Name);
                        tasks += "\n";

                        number++;
                    }
                    await context.PostAsync(tasks);

                    await context.PostAsync("Какую задачу удалить? Напишите номер:");
                    context.Wait(DeleteTask);
                }
            }
            else if (input == "Удалить все задачи")
            {
                todo.Clear();
                await context.PostAsync("Задачи все успешно удалены!");

                PromptDialog.Choice(
                    context: context,
                    resume: SecondReceivedAsync,
                    options: new List<string> { "Добавить задачу", "Посмотреть задачи на сегодня", "Удалить задачу", "Мотивирующая цитата на каждый день", "Настроить напоминание задач" },
                    prompt: $"Привет! Хочешь добавить задачу на сегодня",
                    promptStyle: PromptStyle.Auto);
            }
            else
            {
                int number = 0;
                List<Problem> todo_time = new List<Problem>();
                Datatime thistime = new Datatime();
                for (int i = 0; i < todo.Count; i++)
                {
                    Problem task = todo[i];
                    if (task.Add_time.Day != thistime.Day)
                    {
                        todo_time.Add(task);
                    }
                    number++;
                }
                todo.Clear();
                for (int i = 0; i < todo_time.Count; i++)
                {
                    todo.Add(todo_time[i]);
                }
                todo_time.Clear();
                if (todo.Count > 0)
                {
                    number = 1;
                    string tasks = "";
                    foreach (Problem task in todo)
                    {
                        tasks += String.Format("{0}) {1}", number.ToString(), task.Name);
                        tasks += "\n";

                        number++;
                    }
                    await context.PostAsync(tasks);
                }
                else
                {
                    await context.PostAsync("Задачи все успешно удалены!");
                }
                PromptDialog.Choice(
                context: context,
                resume: SecondReceivedAsync,
                options: new List<string> { "Добавить задачу", "Посмотреть задачи на сегодня", "Удалить задачу", "Мотивирующая цитата на каждый день", "Настроить напоминание задач" },
                prompt: $"Привет! Хочешь добавить задачу на сегодня",
                promptStyle: PromptStyle.Auto);
            }
        }
        private async Task TextReceivedAsync(IDialogContext context, IAwaitable<string> result)
        {
            string input = await result;
            Regex need = new Regex(@"^[0-9]{2}:[0-9]{2}:[0-9]{2}");

            if (need.IsMatch(input))
            {
                string hours = "", minutes = "", seconds = "";
                hours += input[0];
                hours += input[1];
                minutes += input[3];
                minutes += input[4];
                seconds += input[6];
                seconds += input[7];
                int t;
                Int32.TryParse(hours, out t);
                t *= 3600000;
                timeseed += t;
                Int32.TryParse(minutes, out t);
                t *= 60000;
                timeseed += t;
                Int32.TryParse(seconds, out t);
                t *= 1000;
                timeseed += t;

                await MessagesController.Starttimer(timeseed);


                await context.PostAsync(String.Format("Oк! Напоминание каждые {0} установлено. Для прекращения напоминания напишите \"Стоп\"", input));

                PromptDialog.Choice(
                    context: context,
                    resume: SecondReceivedAsync,
                    options: new List<string> { "Добавить задачу", "Посмотреть задачи на сегодня", "Удалить задачу", "Мотивирующая цитата", "Настроить напоминание задач" },
                    prompt: $"Привет! Хочешь добавить задачу на сегодня",
                    promptStyle: PromptStyle.Auto);
            }

            else
            {
                await context.PostAsync("Ввод некоректен. Попробуйте еще раз:");
                PromptDialog.Text(
                    context: context,
                    resume: TextReceivedAsync,
                    prompt: "Введите в формате \"ЧЧ:ММ:СС\" время, через которое будет происходить напоминание:");
            }

        }
        private async Task AddToList(IDialogContext context, IAwaitable<IMessageActivity> result)
        {

            IMessageActivity input2 = await result;
            Problem a = new Problem(input2);
            todo.Add(a);

            PromptDialog.Choice(
            context: context,
            resume: AddToList2,
            options: new List<string> { "Да", "Нет" },
            prompt: $"Добавить еще задачу??",
            promptStyle: PromptStyle.Auto);

        }

        private async Task AddToList2(IDialogContext context, IAwaitable<string> result)
        {
            string input = await result;
            if (input.Contains("Да"))
            {
                await context.PostAsync("Напиши свою задачу:");
                context.Wait(AddToList);
            }
            else
            {
                //await context.PostAsync("Ok");
                PromptDialog.Choice(
                context: context,
                resume: SecondReceivedAsync,
                options: new List<string> { "Добавить задачу", "Посмотреть задачи на сегодня", "Удалить задачу", "Мотивирующая цитата", "Настроить напоминание задач" },
                prompt: $"Привет! Хочешь добавить задачу на сегодня",
                promptStyle: PromptStyle.Auto);
            }
        }
        private async Task DeleteTask(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            int number = 0;
            IMessageActivity input2 = await result;


            int res = 0;

            if (Int32.TryParse(input2.Text, out res))
            {
                if (res <= todo.Count && res >= 1)
                {
                    todo.RemoveAt(res - 1);
                    number = 1;
                    string tasks = "";
                    foreach (Problem task in todo)
                    {
                        tasks += String.Format("{0}) {1}", number.ToString(), task.Name);
                        tasks += "\n";

                        number++;
                    }
                    await context.PostAsync(tasks);
                }
                else
                    await context.PostAsync("Такой задачи нет");
            }

            PromptDialog.Choice(
            context: context,
            resume: SecondReceivedAsync,
            options: new List<string> { "Добавить задачу", "Посмотреть задачи на сегодня", "Удалить задачу", "Мотивирующая цитата на каждый день", "Настроить напоминание задач" },
            prompt: $"Привет! Хочешь добавить задачу на сегодня",
            promptStyle: PromptStyle.Auto);

        }
        //    private async Task OpenToDoList(IDialogContext context, IAwaitable<IMessageActivity> result)
        //{
        //    foreach (string task in todo)
        //        await context.PostAsync(task);
        //}

    }
}