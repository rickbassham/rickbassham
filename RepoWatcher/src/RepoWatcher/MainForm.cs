using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using SharpSvn;

namespace RepoWatcher
{
    public partial class MainForm : Form
    {
        ProjectCollection _projects;

        private Comparison<Revision> _ascRevision = delegate(Revision a, Revision b)
        {
            return a.Number.CompareTo(b.Number);
        };

        private Comparison<Revision> _descRevision = delegate(Revision a, Revision b)
        {
            return b.Number.CompareTo(a.Number);
        };

        public MainForm()
        {
            InitializeComponent();

            _projects = new ProjectCollection();
            Project p = new Project();

            p.Name = "WebMakerX";
            p.Url = new Uri("http://bcsmdvtest2.ucscorp.ucs.pvt:8080/WMX/WebMakerX/Trunk");
            p.MaxHistory = 100;
            p.IgnoreUsers = new List<string>();
            p.CheckEvery = TimeSpan.FromMinutes(10);

            _projects.Add(p);
        }

        protected virtual void OnProjectsUpdated(EventArgs e)
        {
            _projects.Sort(delegate(Project a, Project b) { return a.Name.CompareTo(b.Name); });

            _bindProjectListWorker.RunWorkerAsync();
        }

        private void _repoCheckWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            using (SvnClient client = new SvnClient())
            {
                Project p = _projects[0];

                p.Revisions.Sort(_ascRevision);

                Uri target = p.Url;

                client.Log(target, delegate(object s, SvnLogEventArgs a)
                {
                    Predicate<Revision> findRevision = delegate(Revision r)
                    {
                        return r.Number == a.Revision;
                    };

                    if (p.Revisions.Find(findRevision) == null)
                    {
                        Revision r = new Revision();
                        r.Number = a.Revision;
                        r.Author = a.Author;
                        r.Date = a.Time;
                        r.LogMessage = a.LogMessage;

                        p.Revisions.Add(r);

                        if (p.Revisions.Count >= 100)
                        {
                            a.Cancel = true;
                        }
                    }
                    else
                    {
                        a.Cancel = true;
                    }
                });

                p.Revisions.Sort(_ascRevision);

                while (p.Revisions.Count > 100)
                {
                    p.Revisions.RemoveAt(0);
                }
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            _repoCheckWorker.RunWorkerAsync();
        }

        private void _addProjectMenuItem_Click(object sender, EventArgs e)
        {
            using (EditProjectForm projectForm = new EditProjectForm())
            {
                DialogResult result = projectForm.ShowDialog(this);

                if (result == DialogResult.OK)
                {
                    if (projectForm.Project != null)
                    {
                        _projects.Add(projectForm.Project);
                        OnProjectsUpdated(EventArgs.Empty);
                    }
                }
            }
        }

        private void _bindProjectListWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            _projectList.Nodes.Clear();

            foreach (Project p in _projects)
            {
                _projectList.Nodes.Add(p.Name);
            }
        }
    }
}
