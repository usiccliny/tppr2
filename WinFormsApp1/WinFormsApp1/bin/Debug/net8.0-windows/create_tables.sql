create table if not exists project (
    project_id serial primary key,
    project_name varchar(255) not null,
    start_date date not null,
    end_date date not null,
    status varchar(50) not null,
    constraint uk_project unique (project_name, status)
);
comment on table project is 'Таблица Проекты';
comment on column project.project_id is 'Уникальный идентификатор проекта';
comment on column project.project_name is 'Название проекта';
comment on column project.start_date is 'Дата начала проекта';
comment on column project.end_date is 'Дата окончания проекта';
comment on column project.status is 'Статус проекта (например, активен, завершен)';

create table if not exists employee (
    employee_id serial primary key,
    first_name varchar(100) not null,
    last_name varchar(100) not null,
    email varchar(255) unique not null
);
comment on table employee is 'Таблица Сотрудники';
comment on column employee.employee_id is 'Уникальный идентификатор сотрудника';
comment on column employee.first_name is 'Имя сотрудника';
comment on column employee.last_name is 'Фамилия сотрудника';
comment on column employee.email is 'Уникальный адрес электронной почты сотрудника';

create table if not exists team (
    team_id serial primary key,
    team_name varchar(100) not null,
    lead_employee_id int,
    foreign key (lead_employee_id) references employee(employee_id),
    constraint uk_team unique (team_name, lead_employee_id)
);
comment on table team is 'Таблица Команды';
comment on column team.team_id is 'Уникальный идентификатор команды';
comment on column team.team_name is 'Название команды';
comment on column team.lead_employee_id is 'Идентификатор сотрудника, который является руководителем команды';

create table if not exists task (
    task_id serial primary key,
    task_name varchar(255) not null,
    assigned_to int,
    project_id int,
    status varchar(50) not null,
    due_date date not null,
    foreign key (assigned_to) references employee(employee_id),
    foreign key (project_id) references project(project_id),
    constraint uk_task unique (task_name, assigned_to, project_id, status)
);
comment on table task is 'Таблица Задачи';
comment on column task.task_id is 'Уникальный идентификатор задачи';
comment on column task.task_name is 'Название задачи';
comment on column task.assigned_to is 'Идентификатор сотрудника, которому назначена задача';
comment on column task.project_id is 'Идентификатор проекта, к которому относится задача';
comment on column task.status is 'Статус задачи (например, новая, в процессе, завершена)';
comment on column task.due_date is 'Дата окончания задачи';

create table if not exists project_team (
    project_id int,
    team_id int,
    primary key (project_id, team_id),
    foreign key (project_id) references project(project_id),
    foreign key (team_id) references team(team_id),
    constraint uk_project_team unique (project_id, team_id)
);
comment on table project_team is 'Связь между проектами и командами';
comment on column project_team.project_id is 'Идентификатор проекта';
comment on column project_team.team_id is 'Идентификатор команды';
