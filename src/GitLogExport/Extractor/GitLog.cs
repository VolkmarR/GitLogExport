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
                    ChangeKind.Deleted => FileStatus.Deleted,
                    ChangeKind.Renamed => FileStatus.Renamed,
                    _ => FileStatus.Modified
                }
            };

            return result;
        }

        Commit BuildCommit(LibGit2Sharp.Commit commit)
        {
            var item = new Commit
            {
                Hash = commit.Sha,
                Subject = commit.MessageShort,
                Description = GetDescription(commit),
                TimeStamp = DateTimeOffset.MinValue
            };

            if (commit.Author != null)
            {
                item.Author = new Author { Name = commit.Author.Name, Email = commit.Author.Email };
                item.TimeStamp = commit.Author.When;
            }

            if (commit.Committer != null)
            {
                item.Committer = new Author { Name = commit.Committer.Name, Email = commit.Committer.Email };
                if (item.TimeStamp == DateTimeOffset.MinValue)
                    item.TimeStamp = commit.Committer.When;
            }

            var files = _Repository.Diff.Compare<Patch>(commit.Parents.FirstOrDefault()?.Tree, commit.Tree);

            item.Files = files.Select(GetFile).ToList();

            return item;
        }

        public IEnumerable<Commit> GetCommits(Filter filter)
        {
            if (!string.IsNullOrEmpty(filter?.CommitSha))
            {
                var commit = _Repository.Commits.FirstOrDefault(q => q.Sha == filter.CommitSha);
                if (commit != null)
                    yield return BuildCommit(commit);
                yield break;
            }


            ICommitLog commits = _Repository.Commits;
            if (filter != null)
            {
                var commitFilter = new CommitFilter();
                if (!string.IsNullOrEmpty(filter.CommitsAfterSha))
                    commitFilter.ExcludeReachableFrom = filter.CommitsAfterSha;
                if (!string.IsNullOrEmpty(filter.CommitsToSha))
                    commitFilter.IncludeReachableFrom = filter.CommitsToSha;
                commits = _Repository.Commits.QueryBy(commitFilter);
            }

            foreach (var commit in commits)
                yield return BuildCommit(commit);
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
