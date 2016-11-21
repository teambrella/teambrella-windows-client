/* Copyright(C) 2016  Teambrella, Inc.
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Affero General Public License(version 3) as published
 * by the Free Software Foundation.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Affero General Public License for more details.
 *
 * You should have received a copy of the GNU Affero General Public License
 * along with this program.  If not, see<http://www.gnu.org/licenses/>.
 */
using System.Threading;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Teambrella.Client.DomainModel;

namespace Teambrella.Client.Dal
{
    public class TeambrellaContext : System.Data.Entity.DbContext
    {
        public TeambrellaContext() : base("name=TeambrellaContext")
        {
            // Turn off the Migrations, (NOT a code first Db)
            Database.SetInitializer<TeambrellaContext>(null);
        }
        public System.Data.Entity.DbSet<User> User { get; set; }
        public System.Data.Entity.DbSet<Connection> Connection { get; set; }
        public System.Data.Entity.DbSet<Team> Team { get; set; }
        public System.Data.Entity.DbSet<Teammate> Teammate { get; set; }
        public System.Data.Entity.DbSet<PayTo> PayTo { get; set; }
        public System.Data.Entity.DbSet<BtcAddress> UserAddress { get; set; }
        public System.Data.Entity.DbSet<Cosigner> Cosigner { get; set; }
        public System.Data.Entity.DbSet<Tx> Tx { get; set; }
        public System.Data.Entity.DbSet<TxInput> TxInput { get; set; }
        public System.Data.Entity.DbSet<TxOutput> TxOutput { get; set; }
        public System.Data.Entity.DbSet<TxSignature> TxSignature { get; set; }
        public System.Data.Entity.DbSet<Disbanding> Disbanding { get; set; }
        public System.Data.Entity.DbSet<DisbandingTxSignature> DisbandingTxSignature { get; set; }

        protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder)
        {
            // Database does not pluralize table names
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<PayTo>().HasRequired(x => x.Teammate).WithMany(x => x.PayTos).HasForeignKey(x => x.TeammateId);

            modelBuilder.Entity<Teammate>().HasRequired(x => x.Team).WithMany(x => x.Teammates).HasForeignKey(x => x.TeamId);

            modelBuilder.Entity<Tx>().HasRequired(x => x.Teammate).WithMany().HasForeignKey(x => x.TeammateId);
            modelBuilder.Entity<Tx>().HasOptional(x => x.ClaimTeammate).WithMany().HasForeignKey(x => x.ClaimTeammateId);
            modelBuilder.Entity<Tx>().HasOptional(x => x.MoveToAddress).WithMany(x => x.MoveFundsTxs).HasForeignKey(x => x.MoveToAddressId);

            modelBuilder.Entity<TxInput>().HasRequired(x => x.Tx).WithMany(x => x.Inputs).HasForeignKey(x => x.TxId);

            modelBuilder.Entity<TxOutput>().HasRequired(x => x.Tx).WithMany(x => x.Outputs).HasForeignKey(x => x.TxId);
            modelBuilder.Entity<TxOutput>().HasOptional(x => x.PayTo).WithMany().HasForeignKey(x => x.PayToId);

            modelBuilder.Entity<TxSignature>().HasRequired(x => x.TxInput).WithMany(x => x.Signatures).HasForeignKey(x => x.TxInputId);
            modelBuilder.Entity<TxSignature>().HasRequired(x => x.Teammate).WithMany().HasForeignKey(x => x.TeammateId);

            modelBuilder.Entity<BtcAddress>().HasKey(x => x.Address);
            modelBuilder.Entity<BtcAddress>().HasRequired(x => x.Teammate).WithMany(x => x.Addresses).HasForeignKey(x => x.TeammateId);

            modelBuilder.Entity<Cosigner>().HasKey(x => new { x.AddressId, x.KeyOrder });
            modelBuilder.Entity<Cosigner>().HasRequired(x => x.Address).WithMany(x => x.Cosigners).HasForeignKey(x => x.AddressId);
            modelBuilder.Entity<Cosigner>().HasRequired(x => x.Teammate).WithMany(x => x.CosignerOf).HasForeignKey(x => x.TeammateId);

            modelBuilder.Entity<Disbanding>().HasRequired(x => x.Teammate).WithMany(x => x.Disbandings).HasForeignKey(x => x.TeammateId);

            modelBuilder.Entity<DisbandingTxSignature>().HasRequired(x => x.Cosigner).WithMany(x => x.DisbandingTxSignatures).HasForeignKey(x => new { x.AddressId, x.KeyOrder });
            modelBuilder.Entity<DisbandingTxSignature>().HasRequired(x => x.Address).WithMany().HasForeignKey(x => x.AddressId);
        }

        public override int SaveChanges()
        {
            // Make sure there's no concurrent write to the DB 
            Mutex m = new Mutex(false, "Teambrella_Mutex");
            try
            {
                m.WaitOne();
                int ret = base.SaveChanges();
                return ret;
            }
            finally
            {
                m.ReleaseMutex();
            }
        }
    }
}
