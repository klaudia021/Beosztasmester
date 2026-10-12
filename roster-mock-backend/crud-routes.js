function registerCrud(app, name, seed, idPrefix) {
    const items = [...seed];
    let nextNumber = items.length + 1;

    function newId() {
        const n = nextNumber++;
        return idPrefix ? idPrefix + String(n).padStart(3, '0') : n;
    }

    function findIndex(id) {
        return items.findIndex(item => String(item.id) === id);
    }

    app.get(`/api/${name}`, (req, res) => {
        res.json(items);
    });

    app.get(`/api/${name}/:id`, (req, res) => {
        const index = findIndex(req.params.id);
        if (index === -1) return res.status(404).json({ error: 'not found' });
        res.json(items[index]);
    });

    app.post(`/api/${name}`, (req, res) => {
        const item = { ...req.body, id: newId() };
        items.push(item);
        console.log(`[${name}] created ${item.id}`);
        res.status(201).json(item);
    });

    app.put(`/api/${name}/:id`, (req, res) => {
        const index = findIndex(req.params.id);
        if (index === -1) return res.status(404).json({ error: 'not found' });
        items[index] = { ...req.body, id: items[index].id };
        console.log(`[${name}] updated ${items[index].id}`);
        res.json(items[index]);
    });

    app.delete(`/api/${name}/:id`, (req, res) => {
        const index = findIndex(req.params.id);
        if (index === -1) return res.status(404).json({ error: 'not found' });
        const [removed] = items.splice(index, 1);
        console.log(`[${name}] deleted ${removed.id}`);
        res.status(204).end();
    });
}

module.exports = { registerCrud };