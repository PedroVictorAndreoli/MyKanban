using Microsoft.EntityFrameworkCore;
using MyKanban.Models;

namespace MyKanban.Data;

public class MyKanbanDbContext : DbContext
{
    public MyKanbanDbContext(DbContextOptions<MyKanbanDbContext> options) : base(options)
    {
    }

    public DbSet<Label> Labels => Set<Label>();
    public DbSet<Status> Statuses => Set<Status>();
    public DbSet<KanbanTask> Tasks => Set<KanbanTask>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Label>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(200).IsRequired();
            entity.Property(e => e.ColorBg).HasMaxLength(20).IsRequired();
            entity.Property(e => e.ColorText).HasMaxLength(20).IsRequired();
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).HasMaxLength(200).IsRequired();
        });

        modelBuilder.Entity<KanbanTask>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Description).HasMaxLength(1000).IsRequired();
            entity.Property(e => e.LabelId).IsRequired();
            entity.Property(e => e.StatusId).IsRequired();

            entity.HasOne(e => e.Label)
                .WithMany(l => l.Tasks)
                .HasForeignKey(e => e.LabelId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Status)
                .WithMany(s => s.Tasks)
                .HasForeignKey(e => e.StatusId)
                .OnDelete(DeleteBehavior.Restrict);
        });


        modelBuilder.Entity<Label>().HasData(
            new Label { Id = 1, Name = "Planejamento", ColorBg = "#e0f2fe", ColorText = "#0369a1" },
            new Label { Id = 2, Name = "Design", ColorBg = "#ede9fe", ColorText = "#6d28d9" },
            new Label { Id = 3, Name = "Desenvolvimento", ColorBg = "#dcfce7", ColorText = "#15803d" },
            new Label { Id = 4, Name = "Pesquisa", ColorBg = "#fef9c3", ColorText = "#a16207" },
            new Label { Id = 5, Name = "QA", ColorBg = "#fee2e2", ColorText = "#b91c1c" },
            new Label { Id = 6, Name = "Entrega", ColorBg = "#e2e8f0", ColorText = "#334155" }
        );

        modelBuilder.Entity<Status>().HasData(
            new Status { Id = 1, Title = "A fazer" },
            new Status { Id = 2, Title = "Em progresso" },
            new Status { Id = 3, Title = "Revisão" },
            new Status { Id = 4, Title = "Concluído" }
        );

        modelBuilder.Entity<KanbanTask>().HasData(
            new KanbanTask { Id = 1, Title = "Definir escopo do sprint", Description = "Revisar prioridades com o time e ajustar metas.", LabelId = 1, StatusId = 1 },
            new KanbanTask { Id = 2, Title = "Wireframes da tela inicial", Description = "Alinhar layout e componentes principais.", LabelId = 2, StatusId = 1 },
            new KanbanTask { Id = 3, Title = "Integração com API", Description = "Conectar endpoints e validar respostas.", LabelId = 3, StatusId = 2 },
            new KanbanTask { Id = 4, Title = "Benchmark de concorrentes", Description = "Mapear funcionalidades e diferenciais.", LabelId = 4, StatusId = 2 },
            new KanbanTask { Id = 5, Title = "Testes de usabilidade", Description = "Coletar feedback de usuários internos.", LabelId = 5, StatusId = 3 },
            new KanbanTask { Id = 6, Title = "Configurar CI/CD", Description = "Pipeline básico configurado.", LabelId = 6, StatusId = 4 }
        );

    }
}
