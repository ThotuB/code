pub struct ReadHandler<'r> {
    buffer: &'r String,
}

impl<'r> ReadHandler<'r> {
    pub fn new(text: &'r String) -> Self {
        Self { buffer: text }
    }

    pub fn handle(&self) {
        println!("ReadHandler: {}", self.buffer);
    }
}
