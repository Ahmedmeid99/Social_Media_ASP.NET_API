using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Social_Media_APILayer.Models;

namespace Social_Media_APILayer.Data;

public partial class AppDbcontext : DbContext
{
    public AppDbcontext()
    {
    }

    public AppDbcontext(DbContextOptions<AppDbcontext> options)
        : base(options)
    {
    }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<Post> Posts { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserBackgroundPicture> UserBackgroundPictures { get; set; }

    public virtual DbSet<UserProfilePicture> UserProfilePictures { get; set; }

    public virtual DbSet<UserRelationship> UserRelationships { get; set; }

	public DbSet<ReactionType> ReactionTypes { get; set; } = null!;

	public DbSet<UserReaction> UserReactions { get; set; } = null!;

	public DbSet<PostsView> PostsViews { get; set; }
	public DbSet<CommentsView> CommentsViews { get; set; }

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(Settings.ConnectionString);
        }
    }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Comment>(entity =>
		{
			entity.HasKey(e => e.CommentId).HasName("PK__Comments__C3B4DFCA9E1B775D");

			entity.ToTable(tb => tb.HasTrigger("trg_UpdateComment"));

			entity.HasIndex(e => e.PostId, "idx_Comments_PostId");

			entity.HasIndex(e => e.UserId, "idx_Comments_UserId");

			entity.Property(e => e.CreatedAt)
				.HasDefaultValueSql("(getdate())")
				.HasColumnType("datetime");
			entity.Property(e => e.UpdatedAt)
				.HasDefaultValueSql("(getdate())")
				.HasColumnType("datetime");

			entity.HasOne(d => d.Post).WithMany(p => p.Comments)
				.HasForeignKey(d => d.PostId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__Comments__PostId__5535A963");

			entity.HasOne(d => d.User).WithMany(p => p.Comments)
				.HasForeignKey(d => d.UserId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__Comments__UserId__5629CD9C");
		});

		modelBuilder.Entity<Country>(entity =>
		{
			entity.HasKey(e => e.CountryId).HasName("PK__Countrie__10D1609F5E15B0FB");

			entity.Property(e => e.CountryName).HasMaxLength(250);
		});

		modelBuilder.Entity<Message>(entity =>
		{
			entity.HasKey(e => e.MessageId).HasName("PK__Messages__C87C0C9C37D0D279");

			entity.HasIndex(e => e.ReceiverId, "idx_Messages_ReceiverId");

			entity.HasIndex(e => e.SenderId, "idx_Messages_SenderId");

			entity.Property(e => e.SentAt)
				.HasDefaultValueSql("(getdate())")
				.HasColumnType("datetime");

			entity.HasOne(d => d.Receiver).WithMany(p => p.MessageReceivers)
				.HasForeignKey(d => d.ReceiverId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__Messages__Receiv__6477ECF3");

			entity.HasOne(d => d.Sender).WithMany(p => p.MessageSenders)
				.HasForeignKey(d => d.SenderId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__Messages__Sender__6383C8BA");
		});

		modelBuilder.Entity<Post>(entity =>
		{
			entity.HasKey(e => e.PostId).HasName("PK__Posts__AA126038149C8FBC");

			entity.ToTable(tb => tb.HasTrigger("trg_UpdatePost"));

			entity.HasIndex(e => e.UserId, "idx_Posts_UserID");

			entity.Property(e => e.PostId).HasColumnName("PostID");
			entity.Property(e => e.CreatedAt)
				.HasDefaultValueSql("(getdate())")
				.HasColumnType("datetime");
			entity.Property(e => e.DisLikeCount).HasDefaultValue(0);
			entity.Property(e => e.LikesCount).HasDefaultValue(0);
			entity.Property(e => e.LoveCount).HasDefaultValue(0);
			entity.Property(e => e.MediaType)
				.HasMaxLength(255)
				.HasDefaultValue("None");
			entity.Property(e => e.UpdatedAt)
				.HasDefaultValueSql("(NULL)")
				.HasColumnType("datetime");
			entity.Property(e => e.UserId).HasColumnName("UserID");

			entity.HasOne(d => d.User).WithMany(p => p.Posts)
				.HasForeignKey(d => d.UserId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__Posts__UserID__5070F446");
		});

		modelBuilder.Entity<RelationshipType>(entity =>
		{
			entity.HasKey(e => e.RelationshipTypeId).HasName("PK__Relation__20FE5F818C54D93D");

			entity.HasIndex(e => e.RelationshipTypeName, "UQ__Relation__699DC2AAF45F9D4E").IsUnique();

			entity.Property(e => e.RelationshipTypeName)
				.HasMaxLength(50)
				.IsUnicode(false);
		});

		modelBuilder.Entity<User>(entity =>
		{
			entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4C8800D49A");

			entity.ToTable(tb => tb.HasTrigger("trg_UpdateUser"));

			entity.HasIndex(e => e.PasswordHash, "UQ_PasswordHash").IsUnique();
			entity.HasIndex(e => e.UserName, "UQ_UserName").IsUnique();
			entity.HasIndex(e => e.Email, "UQ__Users__A9D1053495DAF6CC").IsUnique();
			entity.HasIndex(e => e.UserName, "idx_Users_UserName");

			entity.Property(e => e.Address).HasMaxLength(500);
			entity.Property(e => e.CreatedAt)
				.HasDefaultValueSql("(getdate())")
				.HasColumnType("datetime");
			entity.Property(e => e.DateOfBirth).HasColumnType("datetime");
			entity.Property(e => e.Email).HasMaxLength(250);
			entity.Property(e => e.Gender)
				.HasMaxLength(1)
				.IsUnicode(false)
				.IsFixedLength();
			entity.Property(e => e.PasswordHash).HasMaxLength(250);
			entity.Property(e => e.Phone)
				.HasMaxLength(250)
				.IsUnicode(false);
			entity.Property(e => e.UpdatedAt)
				.HasDefaultValueSql("(getdate())")
				.HasColumnType("datetime");
			entity.Property(e => e.UserName).HasMaxLength(250);

			// Define foreign key relationship for Country
			entity.HasOne(d => d.Country)
				.WithMany(p => p.Users)
				.HasForeignKey(d => d.CountryId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__Users__CountryId__4222D4EF");

			// Define relationship for UserProfilePictures (one-to-many)
			entity.HasMany(u => u.UserProfilePictures)
				.WithOne()
				.HasForeignKey(p => p.UserId)
				.OnDelete(DeleteBehavior.Cascade);

			// Define relationship for UserBackgroundPictures (one-to-many)
			entity.HasMany(u => u.UserBackgroundPictures)
				.WithOne()
				.HasForeignKey(b => b.UserId)
				.OnDelete(DeleteBehavior.Cascade);
		});

		modelBuilder.Entity<UserBackgroundPicture>(entity =>
		{
			entity.HasKey(e => e.UserBackgroundPictureId).HasName("PK__UserBack__D21479B330520652");

			entity.ToTable(tb => tb.HasTrigger("trg_UpdateUserBackgroundPicture"));

			entity.Property(e => e.CreatedAt)
				.HasDefaultValueSql("(getdate())")
				.HasColumnType("datetime");
			entity.Property(e => e.UpdatedAt)
				.HasDefaultValueSql("(getdate())")
				.HasColumnType("datetime");

			entity.HasOne(d => d.User).WithMany(p => p.UserBackgroundPictures)
				.HasForeignKey(d => d.UserId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__UserBackg__UserI__29221CFB");
		});

		modelBuilder.Entity<UserProfilePicture>(entity =>
		{
			entity.HasKey(e => e.UserProfilePictureId).HasName("PK__UserProf__55606B5FF4439894");

			entity.ToTable(tb => tb.HasTrigger("trg_UpdateUserProfilePictures"));

			entity.Property(e => e.CreatedAt)
				.HasDefaultValueSql("(getdate())")
				.HasColumnType("datetime");
			entity.Property(e => e.UpdatedAt)
				.HasDefaultValueSql("(getdate())")
				.HasColumnType("datetime");

			entity.HasOne(d => d.User).WithMany(p => p.UserProfilePictures)
				.HasForeignKey(d => d.UserId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__UserProfi__UserI__282DF8C2");
		});

		modelBuilder.Entity<UserRelationship>(entity =>
		{
			entity.HasKey(e => e.RelationshipId).HasName("PK__UserRela__31FEB881111E7B64");

			entity.HasIndex(e => new { e.UserId1, e.UserId2 }, "idx_UserRelationships_UserId1_UserId2");
			
			entity.Property(e => e.CreatedAt)
				.HasDefaultValueSql("(getdate())")
				.HasColumnType("datetime");

			entity.HasOne(d => d.RelationshipType).WithMany(p => p.UserRelationships)
				.HasForeignKey(d => d.RelationshipTypeId)
				.HasConstraintName("FK_UserRelationships_RelationshipTypes");

			entity.HasOne(d => d.UserId1Navigation).WithMany(p => p.UserRelationshipUserId1Navigations)
				.HasForeignKey(d => d.UserId1)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__UserRelat__UserI__49C3F6B7");

			entity.HasOne(d => d.UserId2Navigation).WithMany(p => p.UserRelationshipUserId2Navigations)
				.HasForeignKey(d => d.UserId2)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__UserRelat__UserI__4AB81AF0");

			// Configure ReactionType entity
			modelBuilder.Entity<ReactionType>(entity =>
			{
				entity.HasKey(e => e.ReactionTypeId);
				entity.Property(e => e.ReactionName)
					  .HasMaxLength(50)
					  .IsRequired();
			});

			// Configure UserReaction entity
			modelBuilder.Entity<UserReaction>(entity =>
			{
				entity.HasKey(e => e.ReactionId);

				entity.ToTable(tb =>
				{
					tb.HasTrigger("trg_UpdatePostReactionCount_AfterInsert");
					tb.HasTrigger("trg_UpdatePostReactionCount_AfterUpdate");
					tb.HasTrigger("trg_UpdatePostReactionCount_AfterDelete");

				});

				entity.Property(e => e.CreatedAt)
					  .HasDefaultValueSql("GETDATE()");

				entity.HasOne(d => d.User)
					  .WithMany(p => p.UserReactions)
					  .HasForeignKey(d => d.UserId)
					  .OnDelete(DeleteBehavior.Cascade);

				entity.HasOne(d => d.Post)
					  .WithMany(p => p.UserReactions)
					  .HasForeignKey(d => d.PostId)
					  .OnDelete(DeleteBehavior.Cascade);

				entity.HasOne(d => d.ReactionType)
					  .WithMany(p => p.UserReactions)
					  .HasForeignKey(d => d.ReactionTypeId)
					  .OnDelete(DeleteBehavior.Cascade);
			});
		});

		modelBuilder.Entity<PostsView>(entity =>
		{
			entity.HasNoKey();  // Since it's a view
			entity.ToView("PostsView");  // Map to the actual database view
		});
		
		modelBuilder.Entity<CommentsView>(entity =>
		{
			entity.HasNoKey();  // Since it's a view
			entity.ToView("CommentsView");  // Map to the actual database view
		});

		OnModelCreatingPartial(modelBuilder);
	}

	partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
