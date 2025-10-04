use super::handlers::{handler_read::ReadHandler, handler_write::WriteHandler};

pub struct Service {
    // read_handler: ReadHandler,
    // write_handler: WriteHandler<'w>,
    buffer: String,
}

impl Service {
    pub fn new(text: String) -> Self {
        Self { buffer: text }
    }

    pub fn handle(&mut self) {
        let wh = WriteHandler::new(&mut self.buffer);
        let rh = ReadHandler::new(&self.buffer);
        rh.handle();
        wh.handle();
    }
}
