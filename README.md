## Пример мета игры
Мета игра по типу фермы или тайкуна, где нужно выполнять менеджмент войск, ресурсов и прочие вещи.


### Запуск и точка входа
Запуск - через сцену TestMeta. В MetaDemoInitializer - точка входа.
Можно переключать экраны и нанимать войска до лимита. Верстка сделана для Portrait экрана.

### Основное внимание - MetaCore
Файлы конфигурации и данные отделены от самой игры. Добавлен удобный инструмент настройки конфига для ГД (в процессе ветка OdinConfigs). 
Реализованы максимально мелкие сущности - action, condition для максимально широкой возможности реализаций ГД правил. 
Сделано удобное добавление новых действий и условий для расширения функционала. При реализации придерживался принципов solid.

### Фишки
При "выгрузке" конфига - конвертер соберет конфиг из максимального удобного для работы человека в максимально удобный для работы игры виде.
Можно отдавать ГД настройку игры, без передачи основной части кода (ГД получает только файлы конфига и dto). Отдельно получает билд игры с интрументами, на котором может работать.
При изменении или добавлении полей в конфиг модели игры разработчиками, при попытки выгнузить новую версию из редактора - возникнет ошибка, с описанием того, что изменилось (ошибка всплывет сразу).

### Возможности, которые будут добавлены:
Запись и воспроизведение "реплеев". Воспроизведение багов и ошибок.
Проверка на чининг - на сервере. Откат действий игрока до крайнего валидного.
Логирование и хранение истории действий игрока.


### 
Сознательно не добавлял дополнительных библиотек по типу: zenject, unitask, messagePipe для сохранения минималистичности примера и его понятности.
