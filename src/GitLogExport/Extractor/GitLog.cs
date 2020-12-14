using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace GitLogExport.Extractor
{
    public class GitLog : IDisposable
    {
        readonly string _RepositoryPath;
        readonly Repository _Repository;

        static string GetDescription(LibGit2Sharp.Commit commit)
        {
            if (!string.IsNullOrEmpty(commit.Message) && commit.Message.StartsWith(commit.MessageShort) && commit.Message.Length > commit.MessageShort.Length)
                return (commit.Message.Substring(commit.MessageShort.Length).TrimStart('\n', '\r'));
            return null;
        }

        static File GetFile(PatchEntryChanges patchEntry)
        { 
           var result = new File
               {
                   Path = patchEntry.Path,
                   LinesAdded = patchEntry.LinesAdded,
                   LinesDeleted = patchEntry.LinesDeleted,
                   Status = patchEntry.Status switch 
                   {
                       ChangeKind.Added => FileStatus.Added,
                       ChangeKind.Deleted  => FileStatus.Deleted,
                       ChangeKind.Renamed => FileStatus.Renamed,
                       _ => FileStatus.Modified
                   }
           };

            return result;
        }

        public IEnumerable<Commit> GetCommits()
        {
            foreach (var commit in _Repository.Commits)
            {
                var item = new Commit
                {
                    Hash = commit.Sha,
                    Subject = commit.MessageShort,
                    Description = GetDescription(commit),
                };

                if (commit.Author != null)
                {
                    item.Author = new Author { Name = commit.Author.Name, Email = commit.Author.Email };
                    item.TimeStamp = commit.Author.When;
                }

                var files = _Repository.Diff.Compare<Patch>(commit.Tree, commit.Parents.FirstOrDefault()?.Tree);

                item.Files = files.Select(GetFile).ToList();
                
                yield return item;
            }
        }

        public void Dispose()
        {
            _Repository.Dispose();
        }

        public GitLog(string repositoryPath)
        {
            _RepositoryPath = repositoryPath;
            _Repository = new Repository(_RepositoryPath);
        }


    }
}
