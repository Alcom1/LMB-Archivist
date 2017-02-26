using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LMB_Archivist_Formed
{
    class TaskManager
    {
        //Archiver
        Archiver archiver;

        //Form
        Form1 form;

        //List of tasks to do
        List<ArchiveTask> archiveTasks;

        //Is running state
        bool isRunning = false;

        //Constructor
        public TaskManager(Form1 form)
        {
            this.form = form;
            this.archiver = new Archiver(this, form);
            archiveTasks = new List<ArchiveTask>();
        }

        //
        public void Start()
        {
            RunTask();
        }

        //
        private void RunTask()
        {
            if (archiveTasks.Count > 0)
            {
                var task = archiveTasks[0];

                archiveTasks.RemoveAt(0);

                switch (task.state)
                {
                    case ArchiveOptionState.SaveUserPosts:
                        int targetValue = 0;
                        if (int.TryParse(task.target, out targetValue))
                        {
                            form.NewLine(TextBoxChoice.TextBoxTop);
                            form.Print(TextBoxChoice.TextBoxTop, "TASK START : Save User Posts for ID : " + task.target);
                            form.NewLine(TextBoxChoice.TextBoxBottom);
                            form.Print(TextBoxChoice.TextBoxBottom, "TASK START : Save Topic for URL : " + task.target);
                            archiver.StartUserPostArchiving(targetValue);
                        }
                        else
                        {
                            form.Print(TextBoxChoice.TextBoxTop, "Invalid User ID for task: " + task.target);
                            RunTask();
                        }
                        break;

                    case ArchiveOptionState.SaveUserPages:
                        form.Print(TextBoxChoice.TextBoxTop, "UNIMPLEMENTED FEATURE : ARCHIVE USER PAGES");
                        break;

                    case ArchiveOptionState.SaveUserTopics:
                        form.Print(TextBoxChoice.TextBoxTop, "UNIMPLEMENTED FEATURE : ARCHIVE USER TOPICS");
                        break;

                    case ArchiveOptionState.SaveTopics:
                        form.NewLine(TextBoxChoice.TextBoxTop);
                        form.Print(TextBoxChoice.TextBoxTop, "TASK START : Save Topic for URL : " + task.target);
                        form.NewLine(TextBoxChoice.TextBoxBottom);
                        form.Print(TextBoxChoice.TextBoxBottom, "TASK START : Save Topic for URL : " + task.target);
                        archiver.StartTopicArchiving(task.target);
                        break;
                }
            }
            else
            {
                form.EnableStartButton();
                form.NewLine(TextBoxChoice.TextBoxTop);
                form.Print(TextBoxChoice.TextBoxTop, "No tasks exist to be performed.");
            }
        }

        //
        public void SetFinished()
        {
            form.NewLine(TextBoxChoice.TextBoxBottom);
            form.Print(TextBoxChoice.TextBoxBottom, "Task Completed!");
            RunTask();
        }

        //Validate and add a task to this manager.
        public void AddTask(ArchiveOptionState state, string target)
        {
            archiveTasks.Add(new ArchiveTask()
            {
                state = state,
                target = target
            });
        }
    }
}
