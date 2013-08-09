using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using Cognition.Shared.Changes;

namespace Cognition.Changes.Entity
{
    public class ChangesContext : DbContext
    {
        public DbSet<DocumentChangeNotification> DocumentChangeNotifications { get; set; }

        public ChangesContext()
            : base("DefaultConnection")
        { }
    }
}
