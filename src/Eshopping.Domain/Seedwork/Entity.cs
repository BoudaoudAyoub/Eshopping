﻿using MediatR;
namespace Eshopping.Domain.Seedwork;
public class Entity<T>
{
    private T _Id = default!;

    public virtual T ID
    {
        get => _Id;
        protected set => _Id = value;
    }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public bool IsDeleted { get; set; }

    // Why Use INotification:
    // The use of INotification allows your domain model to dispatch events (domain events) that are 
    // decoupled from the handlers consuming these events. This means the domain model doesn't need 
    // to know about what happens when an event occurs, allowing for easier flexibility and scalability.

    private List<INotification> _domainEvents = default!;
    public IReadOnlyCollection<INotification>? DomainEvents => _domainEvents?.AsReadOnly();

    public void AddDomainEvent(INotification @event)
    {
        _domainEvents ??= [];
        _domainEvents.Add(@event);
    }

    public void RemoveDomainEvents(INotification @event) => _domainEvents?.Remove(@event);

    public void ClearDomainEvents() => _domainEvents?.Clear();
}