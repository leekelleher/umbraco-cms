using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Scoping;
using Umbraco.Cms.Core.Services.Implement;

namespace Umbraco.Cms.Infrastructure.PublishedCache.Persistence
{
    public class NuCacheContentService : RepositoryService, INuCacheContentService
    {
        private readonly INuCacheContentRepository _repository;

        public NuCacheContentService(
            INuCacheContentRepository repository,
            IScopeProvider provider,
            ILoggerFactory loggerFactory,
            IEventMessagesFactory eventMessagesFactory)
            : base(provider, loggerFactory, eventMessagesFactory)
        {
            _repository = repository;
        }

        /// <inheritdoc/>
        public IEnumerable<ContentNodeKit> GetAllContentSources()
            => _repository.GetAllContentSources();

        /// <inheritdoc/>
        public IEnumerable<ContentNodeKit> GetAllMediaSources()
            => _repository.GetAllMediaSources();

        /// <inheritdoc/>
        public IEnumerable<ContentNodeKit> GetBranchContentSources(int id)
            => _repository.GetBranchContentSources(id);

        /// <inheritdoc/>
        public IEnumerable<ContentNodeKit> GetBranchMediaSources(int id)
            => _repository.GetBranchMediaSources(id);

        /// <inheritdoc/>
        public ContentNodeKit GetContentSource(int id)
            => _repository.GetContentSource(id);

        /// <inheritdoc/>
        public ContentNodeKit GetMediaSource(int id)
            => _repository.GetMediaSource(id);

        /// <inheritdoc/>
        public IEnumerable<ContentNodeKit> GetTypeContentSources(IEnumerable<int> ids)
            => _repository.GetTypeContentSources(ids);

        /// <inheritdoc/>
        public IEnumerable<ContentNodeKit> GetTypeMediaSources(IEnumerable<int> ids)
            => _repository.GetTypeContentSources(ids);

        /// <inheritdoc/>
        public void DeleteContentItem(IContentBase item)
            => _repository.DeleteContentItem(item);

        public void DeleteContentItems(IEnumerable<IContentBase> items)
        {
            foreach (IContentBase item in items)
            {
                _repository.DeleteContentItem(item);
            }
        }

        /// <inheritdoc/>
        public void RefreshContent(IContent content)
            => _repository.RefreshContent(content);

        /// <inheritdoc/>
        public void RefreshMedia(IMedia media)
            => _repository.RefreshMedia(media);

        /// <inheritdoc/>
        public void RefreshMember(IMember member)
            => _repository.RefreshMember(member);

        /// <inheritdoc/>
        public void Rebuild(
            IReadOnlyCollection<int> contentTypeIds = null,
            IReadOnlyCollection<int> mediaTypeIds = null,
            IReadOnlyCollection<int> memberTypeIds = null)
        {
            using (IScope scope = ScopeProvider.CreateScope(repositoryCacheMode: RepositoryCacheMode.Scoped))
            {

                scope.ReadLock(Constants.Locks.ContentTree);
                scope.ReadLock(Constants.Locks.MediaTree);
                scope.ReadLock(Constants.Locks.MemberTree);

                _repository.Rebuild(contentTypeIds, mediaTypeIds, memberTypeIds);
                scope.Complete();
            }
        }

        /// <inheritdoc/>
        public bool VerifyContentDbCache()
        {
            using IScope scope = ScopeProvider.CreateScope(autoComplete: true);
            scope.ReadLock(Constants.Locks.ContentTree);
            return _repository.VerifyContentDbCache();
        }

        /// <inheritdoc/>
        public bool VerifyMediaDbCache()
        {
            using IScope scope = ScopeProvider.CreateScope(autoComplete: true);
            scope.ReadLock(Constants.Locks.MediaTree);
            return _repository.VerifyMediaDbCache();
        }

        /// <inheritdoc/>
        public bool VerifyMemberDbCache()
        {
            using IScope scope = ScopeProvider.CreateScope(autoComplete: true);
            scope.ReadLock(Constants.Locks.MemberTree);
            return _repository.VerifyMemberDbCache();
        }
    }
}
